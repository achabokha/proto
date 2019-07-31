using Embily.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Services
{
    public interface ICardHolder
    {
        Task<string> RegisterWithDocs(Application application);

        Task AssignCard(string cardHolderRef, string cardRef);
    }
}
