using Embily.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Services
{
    public interface IProgramService
    {
        Program GetProgramByDomain(string domain);

        Program GetProgramByUserId(string userId);
    }
}
