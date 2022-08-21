namespace Request.API.Applications.Queries
{
    public interface IRequestQueries
    {
        Task<List<LeaveRequestResponse>> GetLeaveRequestByRequestorId();
        Task<List<LeaveRequestResponse>> GetLeaveRequestByApproverId();
        Task<LeaveRequestDetail> GetLeaveRequest(Guid id);
    }
}
