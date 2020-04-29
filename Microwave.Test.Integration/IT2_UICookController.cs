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

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT2_UICookController
    {
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private UserInterface _userInterface;
        private CookController _cookController;
        private ITimer _timer;
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
            _timer = Substitute.For<ITimer>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _powerTube = Substitute.For<IPowerTube>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);

            _cookController.UI = _userInterface;

            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
        }

        [Test]
        public void UIStartCooking()
        {
            _timer.Received().Start(60);
        }

        [Test]
        public void CookControllerCookingIsDone()
        {
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);
            _display.Received().Clear();
        }

        [Test]
        public void UIStopDoorOpened()
        {
            _door.Open();
            _timer.Received().Stop();
        }

        [Test]
        public void UIStopStartCancelPressed()
        {
            _startCancelButton.Press();
            _timer.Received().Stop();
        }
    }
}
