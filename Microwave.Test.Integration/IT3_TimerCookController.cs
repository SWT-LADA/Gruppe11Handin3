using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT3_TimerCookController
    {
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private UserInterface _userInterface;
        private CookController _cookController;
        private Timer _timer;
        private ILight _light;
        private IDisplay _display;
        private IPowerTube _powerTube;

        [SetUp]
        public void SetUp()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _timer = new Timer();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _powerTube = Substitute.For<IPowerTube>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);

            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
        }

        [Test]
        public void OnTimerTick()
        {
            Thread.Sleep(5150);
            _display.Received().ShowTime(0,55);
        }

        [Test]
        public void OnTimerExpired()
        {
            Thread.Sleep(65000);
            _powerTube.Received().TurnOff();
        }
    }
}
