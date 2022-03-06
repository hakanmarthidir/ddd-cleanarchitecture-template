using Domain.Enums;
using Domain.Shared;

namespace Domain.Entities.UserAggregate.Specifications
{
    public class GetRegisteredUserSpec : BaseSpec<Domain.Entities.UserAggregate.User>
    {
        public GetRegisteredUserSpec(Status studentStatus, int page, int pageSize) : base(x => x.Status == studentStatus)
        {
            AddPaging(page, pageSize);
            AddSortByDescendingExpression(x => x.CreatedDate);
        }
    }
}
