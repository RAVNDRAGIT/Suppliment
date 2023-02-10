using DataContract.Home;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Home
{
    public class GoalService
    {
        private IGoalRepository _goalRepository;
        public GoalService(IGoalRepository goalRepository) {
            _goalRepository = goalRepository;
        }
        public async Task<List<GoalDC>> GetActiveGoals()
        {
            var data= await _goalRepository.GetActiveGoalsAsync();
            return data;
        }
    }
}
