﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT4_B_Display
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
        }
        [Test]
        public void ShowPower()
        {
            _powerButton.Press();
            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void ShowTime()
        {
            _powerButton.Press();
            _timeButton.Press();
            _output.Received().OutputLine("Display shows: 01:00");
        }

        //Her mangler en test for det loop, der skal raise et event

        //Mangler også en test for kaldet nederst på side 6, da den er svær at nå

        [Test]
        public void DoorOpenedWhenCooking() //her fandt vi en fejl og rettede den i UI-klassen
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
