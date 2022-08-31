namespace Request.Domain.Entities.Requests
{
    public static class StatusEnum
    {
        public static readonly Guid Waiting = Guid.Parse("1da73895-ee70-4b9f-edbf-08da79bae8d2");
        public static readonly Guid Accept = Guid.Parse("623289d8-6a9a-49d1-9e3d-82c68ae86854");
        public static readonly Guid Refuse = Guid.Parse("a93c07c7-63d5-4e88-943f-a4ac7df4d110");
        public static readonly Guid Cancel = Guid.Parse("6cb44768-9f3d-42c3-b8e5-cbfbabcee339");
    }
}