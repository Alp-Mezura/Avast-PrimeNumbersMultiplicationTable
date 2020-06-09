using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Avast_PrimeNumbersMultiplicationTable.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public int NumberCount;
        public List<int> PrimeNumbers = new List<int>() { };
        public int[,] MultiplicationTable = { };

        /// <summary>
        ///     Constructor Method
        /// </summary>
        /// <param name="logger"></param>
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            // Get number count from appsettings and store to the variable
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            NumberCount = Convert.ToInt32(config["NumberCount"]);            
        }
        /// <summary>
        /// Method handles Post event via submit button
        /// </summary>
        public void OnPost()
        {
            // Process the entered numbers
            try
            {
                ProcessNumbers();
            }
            catch (Exception e)
            {
                _logger.LogError($"An Exception Occured While Processing Numbers: {e.Message}");
            }
            
        }
        /// <summary>
        /// Processes the entered values by user
        /// </summary>
        private void ProcessNumbers()
        {
            string inputValue; 
            bool isInteger;

            // Re-Initialize the Prime Numbers list
            PrimeNumbers = new List<int>() { };
            
            // Iterate entered numbers via For loop
            for (int i = 0; i < NumberCount; i++)
            {
                // Store entered number by user to the relevant variable
                inputValue = HttpContext.Request.Form[$"txtNumber_{i}"];
                // Check is entered number Int
                isInteger = int.TryParse(inputValue, out int inputNumber);
                // If it is Int process it
                if (isInteger)
                {
                    // Add the value to the Prime Numbers list if it is a Prime Number
                    if (IsPrimeNumber(inputNumber))
                    {
                        PrimeNumbers.Add(inputNumber);
                    }
                }
                else
                {
                    // Log the information regarding the entered value is not Int
                    _logger.LogWarning($"Number_{i} value [{inputValue}] is not Integer");   
                }                
            }
            // Create the Multiplication Table with the Prime Number values
            CreateMultiplicationTable();
        }
        /// <summary>
        /// Creates the Multiplication Table
        /// </summary>
        private void CreateMultiplicationTable()
        {
            // Re-Initialize the array
            MultiplicationTable = new int[PrimeNumbers.Count, PrimeNumbers.Count];

            int row = 0;
            foreach (int numberRow in PrimeNumbers)
            {
                int col = 0;
                foreach (int numberCol in PrimeNumbers)
                {
                    // Calculate the multiplication value and store it to the array
                    MultiplicationTable[row, col] = numberRow * numberCol;
                    col++;
                }
                row++;
            }

            

        }
        /// <summary>
        /// Checks whether the number is Prime Number or not
        /// </summary>
        /// <param name="number">The value that will be checked</param>
        /// <returns>true if the number is Prime Number, false if not</returns>
        private bool IsPrimeNumber(int number)
        {
            bool result = false;
            // If the value is greater than 1
            if (number > 1)
            {
                // Check if the value has no positive divisors
                result = Enumerable.Range(1, number)
                                    .Where(x => number % x == 0)
                                    .SequenceEqual(new[] { 1, number });
            }

            return result;
        }
    }
}
