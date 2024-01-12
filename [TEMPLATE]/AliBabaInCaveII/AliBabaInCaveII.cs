using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class AliBabaInCaveII
    {
        #region YOUR CODE IS HERE

        #region FUNCTION#1: Calculate the Value
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given the Camels possible load and N items, each with its weight, profit and number of instances, 
        /// Calculate the max total profit that can be carried within the given camels' load
        /// </summary>
        /// <param name="camelsLoad">max load that can be carried by camels</param>
        /// <param name="itemsCount">number of items</param>
        /// <param name="weights">weight of each item [ONE-BASED array]</param>
        /// <param name="profits">profit of each item [ONE-BASED array]</param>
        /// <param name="instances">number of instances for each item [ONE-BASED array]</param>
        /// <returns>Max total profit</returns>
        public static int SolveValue(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            CheckInputValidity(camelsLoad, itemsCount, weights, profits, instances);
            int[,] dp = InitializeDP(itemsCount, camelsLoad);
            int result = ComputeDP(dp, itemsCount, camelsLoad, weights, profits, instances);
            return result;
        }
        #region Helper funcations for funcation SolveValue
        public static void CheckInputValidity(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            if (camelsLoad < 0 || itemsCount < 0 || weights == null || profits == null || instances == null)
            {
                throw new ArgumentException("Invalid input values");
            }

            if (weights.Length != itemsCount + 1 || profits.Length != itemsCount + 1 || instances.Length != itemsCount + 1)
            {
                throw new ArgumentException("Invalid input arrays");
            }
        }

        public static int[,] InitializeDP(int itemsCount, int camelsLoad)
        {
            int[,] dp = new int[itemsCount + 1, camelsLoad + 1];

            for (int i = 0; i <= itemsCount; i++)
            {
                dp[i, 0] = 0;
            }

            return dp;
        }

        public static int ComputeDP(int[,] dp, int itemsCount, int camelsLoad, int[] weights, int[] profits, int[] instances)
        {
            for (int i = 1; i <= itemsCount; i++)
            {
                for (int j = 0; j <= camelsLoad; j++)
                {
                    dp[i, j] = dp[i - 1, j];

                    for (int k = 1; k <= instances[i]; k++)
                    {
                        if (j >= k * weights[i])
                        {
                            dp[i, j] = Math.Max(dp[i, j], dp[i - 1, j - k * weights[i]] + k * profits[i]);
                        }
                    }
                }
            }

            return dp[itemsCount, camelsLoad];
        }
        #endregion

        #endregion

        #region FUNCTION#2: Construct the Solution
        //Your Code is Here:
        //==================
        /// <returns>Tuple array of the selected items to get MAX profit (stored in Tuple.Item1) together with the number of instances taken from each item (stored in Tuple.Item2)
        /// OR NULL if no items can be selected</returns>

        public static Tuple<int, int>[] ConstructSolution(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            ValidateInputs(camelsLoad, itemsCount, weights, profits, instances);

            int[,] dp = new int[itemsCount + 1, camelsLoad + 1];
            int[,] selectedItems = new int[itemsCount + 1, camelsLoad + 1];

            CalculateDynamicProgrammingTable(camelsLoad, itemsCount, weights, profits, instances, dp, selectedItems);

            List<Tuple<int, int>> selected = SelectItems(itemsCount, camelsLoad, weights, selectedItems);

            selected.Reverse();
            return selected.ToArray();
        }
        #region Helper Funcations for funcation ConstructSolution
        private static void ValidateInputs(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            if (camelsLoad < 0 || itemsCount < 0 || weights == null || profits == null || instances == null)
            {
                throw new ArgumentException("Invalid input values");
            }

            if (weights.Length != itemsCount + 1 || profits.Length != itemsCount + 1 || instances.Length != itemsCount + 1)
            {
                throw new ArgumentException("Invalid input arrays");
            }
        }

        private static void CalculateDynamicProgrammingTable(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances, int[,] dp, int[,] selectedItems)
        {
            for (int i = 1; i <= itemsCount; i++)
            {
                for (int j = 0; j <= camelsLoad; j++)
                {
                    dp[i, j] = dp[i - 1, j];

                    for (int k = 1; k <= instances[i]; k++)
                    {
                        if (j >= k * weights[i])
                        {
                            int newVal = dp[i - 1, j - k * weights[i]] + k * profits[i];
                            if (newVal > dp[i, j])
                            {
                                dp[i, j] = newVal;
                                selectedItems[i, j] = k;
                            }
                        }
                    }
                }
            }
        }

        private static List<Tuple<int, int>> SelectItems(int itemsCount, int camelsLoad, int[] weights, int[,] selectedItems)
        {
            List<Tuple<int, int>> selected = new List<Tuple<int, int>>();
            int currentItem = itemsCount;
            int currentLoad = camelsLoad;

            while (currentItem > 0 && currentLoad > 0)
            {
                int instancesTaken = selectedItems[currentItem, currentLoad];
                if (instancesTaken > 0)
                {
                    selected.Add(Tuple.Create(currentItem, instancesTaken));
                    currentLoad -= instancesTaken * weights[currentItem];
                }
                currentItem--;
            }

            return selected;
        }
        #endregion

        #endregion

        #endregion
    }
}
