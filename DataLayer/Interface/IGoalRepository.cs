using BusinessLayer.Home;
using DataContract.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IGoalRepository
    {
        Task<List<GoalDC>> GetActiveGoalsAsync();
        Task<long> Save(List<Goal> goals);
        Task<long> AddGoal(Goal goal);
    }
}
