using OpenQA.Selenium;

namespace WebScraper.Utils
{
    public static class MimicHumanUtil
    {
        /// <summary>
        /// Enters text input by mimicing human keystrokes to prevent bot detection
        /// </summary>
        /// <param name="word">The text input string to type</param>
        /// <param name="textInputElement">The HTML text input element</param>
        /// <param name="minDelay">Lower bound in milliseconds for delay between automated keystrokes</param>
        /// <param name="maxDelay">Upper bound in milliseconds for delay between automated keystrokes</param>
        public static void EnterTextInputLikeHuman(string word, IWebElement textInputElement, int minDelay = 50, int maxDelay = 200)
        {
            foreach (char letter in word)
            {
                textInputElement.SendKeys(letter.ToString());
                Thread.Sleep(new Random().Next(minDelay, maxDelay));
            }
        }
    }
}
