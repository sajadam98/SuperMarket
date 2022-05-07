using FluentAssertions;
using SuperMarket._Test.Tools.EntryDocuments;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش سند ورود")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "سند ورود را ویرایش کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class EditEntryDocument : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    
    public EditEntryDocument(ConfigurationFixture configuration) : base(configuration)
    {
        _dbContext = CreateDataContext();
    }
    
    [Given("")]
}