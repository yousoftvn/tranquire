﻿using OpenQA.Selenium.Firefox;
using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using ToDoList.Automation.Actions;
using System.Threading;
using OpenQA.Selenium;
using Xunit.Abstractions;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace ToDoList.Specifications
{
    public class ToDoListSteps
    {
        private readonly StringBuilder _reportingStringBuilder;
        public ScenarioContext Context { get; }

        public ToDoListSteps(ScenarioContext context)
        {
            Context = context;
            _reportingStringBuilder = new StringBuilder();
        }

        [BeforeScenario]
        public void Before()
        {
            var driver = new ChromeDriver();
            var screenshotName = Context.ScenarioInfo.Title;
            int i = 0;
#if DEBUG
            var delay = TimeSpan.FromSeconds(1);
#else
            var delay = TimeSpan.Zero;
#endif
            var actor = new Actor("John")
                            .WithReporting(new InMemoryObserver(_reportingStringBuilder))
                            .TakeScreenshots(() => NextScreenshotName(ref i, screenshotName))                             
                            .HighlightTargets()
                            .SlowSelenium(delay)
                            .CanUse(BrowseTheWeb.With(driver));
            Context.Set(actor);
            Context.Set(driver);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            actor.Given(Open.TheApplication());
        }

        private string NextScreenshotName(ref int i, string screenshotName)
        {
            return $"{screenshotName}_{Interlocked.Increment(ref i)}.jpg";
        }

        [AfterScenario]
        public void After()
        {
            Context.Get<ChromeDriver>().Dispose();
            //Context.Get<ITestOutputHelper>().WriteLine(_reportingStringBuilder.ToString());
            Debug.WriteLine(_reportingStringBuilder.ToString());
        }

        [Given(@"I have an empty to-do list")]
        public void GivenIHaveAnEmptyTo_DoList()
        {
        }

        [Given(@"I add the item ""(.*)""")]
        public void GivenIAddTheItem(string item)
        {
            Context.Actor().Given(ToDoItem.AddAToDoItem(item));
        }

        [When(@"I add the item ""(.*)""")]
        public void WhenIAddTheItem(string item)
        {
            Context.Actor().When(ToDoItem.AddAToDoItem(item));
        }

        [Given(@"I have a list with the items ""(.*)""")]
        public void GivenIHaveAListWithTheItems(ImmutableArray<string> items)
        {
            Context.Actor().Given(ToDoItem.AddToDoItems(items));
        }

        [When(@"I remove the item ""(.*)""")]
        public void WhenIRemoveTheItem(string item)
        {
            Context.Actor().When(ToDoItem.RemoveAToDoItem(item));
        }
    }
}
