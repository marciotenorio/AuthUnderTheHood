using System;
using Microsoft.AspNetCore.Authorization;

namespace Web_App.Authorization;

//Necessary for custom requirement handling
public class HRManagerProbationRequirement : IAuthorizationRequirement
{
    public int ProbationMonths { get; }

    public HRManagerProbationRequirement(int probationMonths)
    {
        ProbationMonths = probationMonths;
    }
}
