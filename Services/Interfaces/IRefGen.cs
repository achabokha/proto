
namespace Embily.Services
{
    public interface IRefGen
    {
        string GenTxnRef(long num);

        string GenTxnGroupRef(long num);

        string GenAppRef(long num);
    }
}
