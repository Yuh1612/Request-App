using Dapper;
using Request.Domain.Entities.Requests;
using Request.Domain.Entities.Users;
using Request.Infrastructure.Data;
using System.Linq;

namespace Request.API.Applications.Queries
{
    public class RequestQueries : IRequestQueries
    {
        private readonly ApplicationDbContext _dbContext;

        public RequestQueries(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LeaveRequestReponse>> GetLeaveRequestByUserId(string requestorId)
        {
            using var connection = _dbContext.GetConnection();

            string query = $@"SELECT *
                    FROM dbo.LeaveRequests as r
                    LEFT JOIN dbo.Statuses    AS s
                    ON (
                        r.StatusId  = s.Id
                    )
					LEFT JOIN dbo.Users	AS u
                    ON (
                        r.ApproverId	= u.Id
                    )
					INNER JOIN dbo.Stages	AS st
                    ON (
                        r.Id		= st.LeaveRequestId
                    )";
            var results = await connection.QueryAsync<LeaveRequest, Status, User, Stage, LeaveRequest> (query,
                (request, status, user, stage) => {
                    request.Status = status;
                    request.Approver = user;
                    request.Stages = request.Stages ?? new List<Stage>();
                    request.Stages.Add(stage);
                    return request;
                });
            return MapperLeaveRequests(results.Where(c => c.RequestorId.ToString() == requestorId).ToList());
        }
        public List<LeaveRequestReponse> MapperLeaveRequests(List<LeaveRequest> results)
        {
            List<LeaveRequestReponse> leaveRequests = new List<LeaveRequestReponse>();
            foreach (var item in results)
            {
                LeaveRequestReponse leaveRequestReponse = new LeaveRequestReponse();
                leaveRequestReponse.StatusId = item.StatusId;
                leaveRequestReponse.StatusName = item.Status.Name;
                leaveRequestReponse.ApproverId = item.Approver.Id;
                leaveRequestReponse.ApproverName = item.Approver.UserName;
                leaveRequestReponse.Name = item.Name;
                leaveRequestReponse.StageName = item.Stages.First().Name;
                leaveRequests.Add(leaveRequestReponse);
            }
            return leaveRequests;
        }
    }
}