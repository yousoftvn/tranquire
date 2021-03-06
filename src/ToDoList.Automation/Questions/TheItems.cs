﻿using System;
using Tranquire;
using System.Collections.Immutable;
using Tranquire.Selenium;

namespace ToDoList.Automation.Questions
{
    public static class TheItems
    {
        public static IQuestion<ImmutableArray<Model.ToDoItem>> Displayed()
        {
            return new DisplayedItems();
        }

        public static IQuestion<int> Remaining()
        {
            return new RemainingItems();
        }
    }
}