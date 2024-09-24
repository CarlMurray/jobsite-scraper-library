using OpenQA.Selenium;

namespace WebScraper.Utils
{
    public static class JavaScriptUtil
    {
        /// <summary>
        /// Executes JavaScript in the browser to get the textContent of a job post. Note: Selenium's built in .Text property may not capture data correctly on a LinkedIn job post - use this method to ensure all data is captured.
        /// </summary>
        /// <returns>Returns the textContent for the job post</returns>
        public static string GetTextContentFromScript(string elementCssSelector, WebDriver driver)
        {
            // Ensures the element exists before executing script
            driver.FindElement(By.CssSelector(elementCssSelector));
            string content = driver.ExecuteScript(
                $"let contentElement = document.querySelector('{elementCssSelector}');" +
                "let textContent = contentElement.innerText;" +
                "console.log(textContent);" +
                "return textContent;").ToString()!;
            return content;
        }
    }
}
