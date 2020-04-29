﻿using System;
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
    class IT4_B_DisplayUICookController
    {
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private UserInterface _userInterface;
        private CookController _cookController;
        private Timer _timer;
        private Light _light;
        private Display _display;
        private IPowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _timer = new Timer();
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
            _display = new Display(_output);
            _powerTube = Substitute.For<IPowerTube>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);

            _cookController.UI = _userInterface;
        }
        [Test]
        public void ShowPower()
        {
            _powerButton.Press();
            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void ShowTimeWhenSettingTime()
        {
            _powerButton.Press();
            _timeButton.Press();
            _output.Received().OutputLine("Display shows: 01:00");
        }

        [Test]
        public void ShowTimeWhenCooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(5200);
            _output.Received().OutputLine("Display shows: 00:55");
        }

        [Test]
        public void ClearWhenCookingIsDone()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(65000);
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void DoorOpenedWhenCooking() // Her fandt vi en fejl og rettede den i UI-klassen
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();
            _output.Received().OutputLine("Display cleared");
        }
        [Test]
        public void StartCancelledButtonPressedWhenCooking() 
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine("Display cleared");
        }
    }
}
