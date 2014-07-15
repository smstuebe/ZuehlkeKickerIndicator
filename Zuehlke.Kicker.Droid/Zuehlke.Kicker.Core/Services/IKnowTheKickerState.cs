using System.Threading.Tasks;

namespace Zuehlke.Kicker.Core.Services
{
    public interface IKnowTheKickerState
    {
        Task<KickerState> Current();
    }
}
