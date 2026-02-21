using Discounts.Persistence.Context;
using Application.Interfaces.Repos;
using Domain.Entities;

namespace Discounts.Persistence.Repositories;

public class GlobalSettingsRepository : BaseRepository<GlobalSettings>, IGlobalSettingsRepository
{
    public GlobalSettingsRepository(DiscountsDbContext context) : base(context) { }
}
