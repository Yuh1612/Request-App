namespace Request.API.Applications.Queries
{
    public interface IRequestQueries
    {
        Task<List<LeaveRequestReponse>> GetLeaveRequestByUserId(string id);
    }
}
