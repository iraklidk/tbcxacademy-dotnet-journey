using Domain.Entities;
using Application.Interfaces.Repos;
using Discounts.Persistence.Context;

namespace Discounts.Persistence.Repositories;

public class GlobalSettingsRepository : BaseRepository<GlobalSettings>, IGlobalSettingsRepository
{
    public GlobalSettingsRepository(DiscountsDbContext context) : base(context) { }
}
