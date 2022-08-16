using Dapper;
using Microsoft.Data.SqlClient;
using Request.Domain.Entities.Requests;
using Request.Infrastructure.Data;

namespace Request.API.Applications.Queries
{
    public class RequestQueries: IRequestQueries
    {
        private readonly ApplicationDbContext _dbContext;

        public RequestQueries(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LeaveRequestReponse>> GetLeaveRequestByUserId(Guid id)
        {
            using var connection = _dbContext.GetConnection();
            connection.Open();

            string query = $@"SELECT *
                    FROM dbo.LeaveRequestsn as r
                    LEFT JOIN dbo.Statuses    AS s
                    ON (
                        r.StatusId  = s.Id
                    )
                    LEFT JOIN dbo.Users	AS u
                    ON (
                        r.RequestorId	= u.Id
                    )
					RIGHT JOIN dbo.Stages	AS st
                    ON (
                        r.Id		= st.LeaveRequestId
                    )
                    WHERE r.RequestorId = @requestorId";
            var results = await connection.QueryAsync<LeaveRequest>(query, new
            {
                requestorId = id
            });
            return MapperLeaveRequests(results.ToList());
        }
        public List<LeaveRequestReponse> MapperLeaveRequests(List<LeaveRequest> results)
        {
            List<LeaveRequestReponse> leaveRequests = new List<LeaveRequestReponse>();
            foreach (var item in results)
            {
                LeaveRequestReponse leaveRequestReponse = new LeaveRequestReponse();
                leaveRequestReponse.RequestorId = item.RequestorId;
                leaveRequestReponse.RequestorName = item.Requestor.UserName;
                leaveRequestReponse.StatusId = item.StatusId;
                leaveRequestReponse.StatusName = item.Status.Name;
                leaveRequestReponse.Approver = item.Stages.First().UserId;
                leaveRequestReponse.LeaveRequestName = item.CompensationDayStart.ToString() == null ? "Nghỉ phép" : "Nghỉ và làm bù";
                leaveRequests.Add(leaveRequestReponse);
            }
            return leaveRequests;
        }
    }
}
