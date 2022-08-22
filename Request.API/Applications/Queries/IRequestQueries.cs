namespace Request.API.Applications.Queries
{
    public interface IRequestQueries
    {
        Task<List<LeaveRequestResponse>> GetLeaveRequestByRequestorId(Guid requestorId);

        Task<List<LeaveRequestResponse>> GetLeaveRequestByApproverId(Guid approverId);

        Task<LeaveRequestDetail> GetLeaveRequest(Guid id, Guid userId);
    }
}