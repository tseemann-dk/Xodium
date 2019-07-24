using System;
using Moq;

namespace Xodium.Mvvm.Xamarin.Test.UnitTests
{
    public class ViewModelTestbed
    {
        private readonly Type viewType;

        public ViewModelTestbed(IExecutionEnvironment environment, Type viewType)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.viewType = viewType;
        }

        public IExecutionEnvironment Environment { get; }

        public void VerifyDidGoTo(Mock<IViewModel> mock, Func<Times> times) => VerifyDidGoTo(mock, times());
        public void VerifyDidGoFrom(Mock<IViewModel> mock, Func<Times> times) => VerifyDidGoFrom(mock, times());
        public void VerifyDidGoBackTo(Mock<IViewModel> mock, Func<Times> times) => VerifyDidGoBackTo(mock, times());
        public void VerifyDidGoBackFrom(Mock<IViewModel> mock, Func<Times> times) => VerifyDidGoBackFrom(mock, times());
        public void VerifyDidGoTo(Mock<IViewModel> mock, Times times) => mock.Verify(vm => vm.NavigateTo(), times);
        public void VerifyDidGoFrom(Mock<IViewModel> mock, Times times) => mock.Verify(vm => vm.NavigateFrom(), times);
        public void VerifyDidGoBackTo(Mock<IViewModel> mock, Times times) => mock.Verify(vm => vm.NavigateBackTo(), times);
        public void VerifyDidGoBackFrom(Mock<IViewModel> mock, Times times) => mock.Verify(vm => vm.NavigateBackFrom(), times);
        public void VerifyDidNotGoTo(Mock<IViewModel> mock) => mock.Verify(vm => vm.NavigateTo(), Times.Never);
        public void VerifyDidNotGoFrom(Mock<IViewModel> mock) => VerifyDidGoFrom(mock, Times.Never);
        public void VerifyDidNotGoBackTo(Mock<IViewModel> mock) => VerifyDidGoBackTo(mock, Times.Never);
        public void VerifyDidNotGoBackFrom(Mock<IViewModel> mock) => VerifyDidGoBackFrom(mock, Times.Never);

        public void VerifyDidNotEnter(Mock<IViewModel> mock)
        {
            VerifyDidNotGoTo(mock);
            VerifyDidNotGoBackTo(mock);
        }

        public void VerifyDidNotLeave(Mock<IViewModel> mock)
        {
            VerifyDidNotGoFrom(mock);
            VerifyDidNotGoBackFrom(mock);
        }

        public Mock<IViewModel> CreateViewModelMock()
        {
            var mock = new Mock<IViewModel>();
            mock.Setup(x => x.ExecutionEnvironment).Returns(Environment);

            var viewRegistry = Environment.GetService<IViewRegistry>();
            viewRegistry.RegisterViewType(viewType, mock.Object.GetType());
            return mock;
        }
    }
}
