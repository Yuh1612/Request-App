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

        public async Task<List<LeaveRequestReponse>> GetLeaveRequestByUserId(string requestorId)
        {
            using var connection = _dbContext.GetConnection();

            string query = $@"SELECT r.Id,
                    r.RequestorId,
                    u.UserName as RequestorName,
                    r.StatusId,
                    s.Name as StatusName,
                    r.CompensationDayStart,
                    st.UserId
                    FROM dbo.LeaveRequests as r
                    LEFT JOIN dbo.Statuses    AS s
                    ON (
                        r.StatusId  = s.Id
                    )
                    LEFT JOIN dbo.Users	AS u
                    ON (
                        r.RequestorId	= u.Id
                    )
					LEFT JOIN dbo.Stages	AS st
                    ON (
                        r.Id		= st.LeaveRequestId
                    )
                    WHERE r.RequestorId = @requestorId";
            var results = await connection.QueryAsync<dynamic>(query, new
            {
                requestorId
            });
            return MapperLeaveRequests(results.ToList());
        }
        public List<LeaveRequestReponse> MapperLeaveRequests(dynamic results)
        {
            List<LeaveRequestReponse> leaveRequests = new List<LeaveRequestReponse>();
            foreach (var item in results)
            {
                LeaveRequestReponse leaveRequestReponse = new LeaveRequestReponse();
                leaveRequestReponse.RequestorId = item.RequestorId;
                leaveRequestReponse.RequestorName = item.RequestorName;
                leaveRequestReponse.StatusId = item.StatusId;
                leaveRequestReponse.StatusName = item.StatusName;
                leaveRequestReponse.Approver = item.UserId;
                leaveRequestReponse.LeaveRequestName = item.CompensationDayStart.ToString() == null ? "Nghỉ phép" : "Nghỉ và làm bù";
                leaveRequests.Add(leaveRequestReponse);
            }
            return leaveRequests;
        }
    }
}
