using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_ButtonDoorUI
    {
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private UserInterface _userInterface;
        private ICookController _cookController;
        private ILight _light;
        private IDisplay _display;

        [SetUp]
        public void SetUp()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _cookController = Substitute.For<ICookController>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void DoorOpened()
        {
            _door.Open();
            _light.Received().TurnOn();
        }

        [Test]
        public void DoorClosed()
        {
            _door.Open();
            _door.Close();
            _light.Received().TurnOff();
        }

        [Test]
        public void PowerButtonPressed()
        {
            _powerButton.Press();
            _display.Received().ShowPower(50);
        }

        [Test]
        public void TimeButtonPressed()
        {
            _powerButton.Press();
            _timeButton.Press();
            _display.Received().ShowTime(1,0);
        }

        [Test]
        public void PressStartCancelButton()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _light.Received().TurnOn();
            _cookController.Received().StartCooking(50,60);
        }
    }
}
