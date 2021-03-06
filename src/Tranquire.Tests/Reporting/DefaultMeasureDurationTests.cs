﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting
{
    public class DefaultMeasureDurationTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DefaultMeasureDuration));
        }

        [Theory, DomainAutoData]
        public void MeasureTime_ShouldReturnValue(
            DefaultMeasureDuration sut,
            object expected)
        {
            //arrange
            //act
            var actual = sut.Measure(() => expected);
            //assert
            Assert.Equal(expected, actual.Item2);
        }
        
        [Theory, DomainAutoData]
        public void MeasureTime_ShouldReturnCorrectDuration(
            DefaultMeasureDuration sut,
            object value,
            TimeSpan wait)
        {
            //arrange
            //act
            var actual = sut.Measure(() =>
            {
                Thread.Sleep((int)wait.TotalMilliseconds);
                return value;
            });
            //assert
            actual.Item1.Should().BeGreaterOrEqualTo(wait);
        }
    }
}
