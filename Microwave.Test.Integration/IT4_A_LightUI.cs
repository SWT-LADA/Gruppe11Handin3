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
    [TestFixture()]
    class IT4_A_LightUI
    {
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private UserInterface _userInterface;
        private CookController _cookController;
        private Timer _timer;
        private Light _light;
        private IDisplay _display;
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
            _display = Substitute.For<IDisplay>();
            _powerTube = Substitute.For<IPowerTube>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);

            _cookController.UI = _userInterface;
        }

        [Test]
        public void DoorOpened()
        {
           _door.Open();
           _output.Received().OutputLine("Light is turned on");
        }
        [Test]
        public void DoorClosed()
        {
            _door.Open();
            _door.Close();
            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void StartCancelledPressed()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine("Light is turned on");
        }

        //[Test]
        //public void TurnOffWhenCookingIsDone()
        //{
        //    _powerButton.Press();
        //    _timeButton.Press();
        //    _startCancelButton.Press();
        //    Thread.Sleep(65000);
        //    _output.Received().OutputLine("Light is turned off");
        //}

        [Test]
        public void StartCancelledPressedWhenCooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _startCancelButton.Press();
            _output.Received().OutputLine("Light is turned off");
        }
    }
}
