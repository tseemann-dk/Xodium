using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Unity;
using Xamarin.Forms;
using Xodium.Injection.Unity;
using Xodium.Mvvm.Xamarin.Services;
using Xodium.Mvvm.Xamarin.Test.TestDoubles;
using Xunit;

namespace Xodium.Mvvm.Xamarin.Test.UnitTests
{
    public class NavigationTest
    {
        private readonly IExecutionEnvironment environment;
        private readonly ViewModelTestbed testbed;
        private readonly INavigationService navigationService;

        public NavigationTest()
        {
            var container = new UnityDependencyContainer(new UnityContainer());
            environment = new ExecutionEnvironment(() => container);

            var viewRegistry = new ViewRegistry();
            container.RegisterInstance<IViewRegistry>(viewRegistry);

            var navigation = new NavigationFake();
            var popupNavigation = new PopupNavigationFake();
            var ns = new NavigationService(navigation, popupNavigation, () => viewRegistry);

            navigation.PagePopped += async (s, e) => await ns.OnPagePopped(e.Page);
            container.RegisterInstance<INavigationService>(ns);

            navigationService = environment.NavigationService;
            testbed = new ViewModelTestbed(environment, typeof(ContentView));
        }

        [Fact]
        public async Task CanGoTo()
        {
            var vm = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm.Object);

            testbed.VerifyDidGoTo(vm, Times.Once);
            testbed.VerifyDidNotLeave(vm);
        }

        [Fact]
        public async Task CanGoToAndBack()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidNotGoBackFrom(vm2);
            testbed.VerifyDidNotGoBackTo(vm1);

            await navigationService.GoBack();

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoBackFrom(vm2, Times.Once);
            testbed.VerifyDidGoBackTo(vm1, Times.Once);
        }

        [Fact]
        public async Task CanGoToAndRestart()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.RestartAt(vm2.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidNotLeave(vm2);
        }

        [Fact]
        public async Task CanOpenPopup()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.OpenPopup(vm2.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
        }

        [Fact]
        public async Task CanOpenPopupAndGoBack()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.OpenPopup(vm2.Object);
            await navigationService.GoBack();

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoBackFrom(vm2, Times.Once);
            testbed.VerifyDidGoBackTo(vm1, Times.Once);
        }

        [Fact]
        public async Task CanGoToAndRestartAndSignalAffectedVMs()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.RestartAt(vm3.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoTo(vm3, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once); // Only one, when navigating to vm2, not when restarting at vm3
            testbed.VerifyDidGoFrom(vm2, Times.Once);
            testbed.VerifyDidNotLeave(vm3);
        }

        [Fact]
        public async Task CanGoBackAThousandTimes()
        {
            var vms = new List<Mock<IViewModel>>();
            for (int i = 0; i < 1000; i++)
            {
                vms.Add(testbed.CreateViewModelMock());
            }

            for (int i = 0; i < vms.Count; i++)
            {
                await navigationService.GoTo(vms[i].Object);
            }

            for (int i = 0; i < vms.Count; i++)
            {
                navigationService.CanGoBack.Should().BeTrue();
                await navigationService.GoBack();
            }
        }

        [Fact]
        public async Task CanGoBackReturnsAsExpected()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm4.Object);

            navigationService.CanGoBack.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.CanGoBack.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.CanGoBack.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.CanGoBack.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.CanGoBack.Should().BeFalse();
        }

        [Fact]
        public async Task CanGoBackExactlyOneStepBeyondRoot()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);

            navigationService.CanGoBack.Should().BeTrue();
            await navigationService.GoBack(); // Go to vm1

            navigationService.CanGoBack.Should().BeTrue();
            await navigationService.GoBack(); // Go to somewhere-we-don't-know-prior-to-vm1

            navigationService.CanGoBack.Should().BeFalse();

            (await navigationService
                .Awaiting(async n => await n.GoBack())
                .Should()
                .ThrowAsync<NavigationException>())
                .WithMessage("Cannot navigate back");
        }

        [Fact]
        public async Task CanGoBackExactlyOneStepAfterGoBackToRoot()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoBackToRoot();

            navigationService.CanGoBack.Should().BeTrue();
            await navigationService.GoBack(); // Go to somewhere-we-don't-know-prior-to-vm1

            navigationService.CanGoBack.Should().BeFalse();

            (await navigationService
                .Awaiting(async n => await n.GoBack())
                .Should()
                .ThrowAsync<NavigationException>())
                .WithMessage("Cannot navigate back");
        }


        [Fact]
        public async Task CanDetectIsAtRootSimple()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack(); // Go to vm1

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack(); // Go to somewhere-we-don't-know-prior-to-vm1

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootModal()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.OpenModal(vm1.Object);
            await navigationService.OpenModal(vm2.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack(); // Go to vm1

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack(); // Go to somewhere-we-don't-know-prior-to-vm1

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootPopup()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.OpenPopup(vm1.Object);
            await navigationService.OpenPopup(vm2.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack(); // Go to vm1

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack(); // Go to somewhere-we-don't-know-prior-to-vm1

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootComposite()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.OpenModal(vm2.Object);
            await navigationService.OpenPopup(vm3.Object);
            await navigationService.GoTo(vm4.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack(); // Go to somewhere-we-don't-know-prior-to-vm1

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootCompositeMixed()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.OpenModal(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.OpenPopup(vm3.Object);
            await navigationService.GoTo(vm4.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack(); // Go to somewhere-we-don't-know-prior-to-vm1

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootReusedVM()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm1.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootAfterGoBackToRoot()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoBackToRoot();

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootAfterGoBackToRootAndThenSome()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();
            var vm5 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoBackToRoot();
            await navigationService.GoTo(vm4.Object);
            await navigationService.GoTo(vm5.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectIsAtRootAfterRestartAt()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();
            var vm5 = testbed.CreateViewModelMock();
            var vm6 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.RestartAt(vm4.Object);
            await navigationService.GoTo(vm5.Object);
            await navigationService.GoTo(vm6.Object);

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeTrue();
            await navigationService.GoBack();

            navigationService.IsAtRoot.Should().BeFalse();
        }

        [Fact]
        public async Task CanGoBackMultiple()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm4.Object);

            await navigationService.GoBack(2);

            // Going forward
            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoFrom(vm2, Times.Once);
            testbed.VerifyDidGoTo(vm3, Times.Once);
            testbed.VerifyDidGoFrom(vm3, Times.Once);
            testbed.VerifyDidGoTo(vm4, Times.Once);
            testbed.VerifyDidGoFrom(vm4, Times.Never);
            // Going back
            testbed.VerifyDidGoBackFrom(vm4, Times.Once);
            testbed.VerifyDidGoBackTo(vm3, Times.Once);
            testbed.VerifyDidGoBackFrom(vm3, Times.Once);
            testbed.VerifyDidGoBackTo(vm2, Times.Once);
            // Didn't go too far
            testbed.VerifyDidGoBackFrom(vm2, Times.Never);
            testbed.VerifyDidGoBackTo(vm1, Times.Never);
        }

        [Fact]
        public async Task CanGoBackAndForth()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoBack(); // vm2
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm4.Object);
            await navigationService.GoBack(); // vm3
            await navigationService.GoTo(vm4.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoTo(vm3, Times.Exactly(2));
            testbed.VerifyDidGoTo(vm4, Times.Exactly(2));
            testbed.VerifyDidGoBackTo(vm4, Times.Never);
            testbed.VerifyDidGoBackTo(vm3, Times.Once);
            testbed.VerifyDidGoBackTo(vm2, Times.Once);
            testbed.VerifyDidGoBackTo(vm1, Times.Never);
        }

        [Fact]
        public async Task CanGoToAndComeBackLaterAndStillGoBackAsExpected()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm4.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm4.Object);

            await navigationService.GoBack(7);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Exactly(2));
            testbed.VerifyDidGoTo(vm3, Times.Exactly(3));
            testbed.VerifyDidGoTo(vm4, Times.Exactly(2));
            testbed.VerifyDidGoBackTo(vm4, Times.Once);
            testbed.VerifyDidGoBackTo(vm3, Times.Exactly(3));
            testbed.VerifyDidGoBackTo(vm2, Times.Exactly(2));
            testbed.VerifyDidGoBackTo(vm1, Times.Once);
        }

        [Fact]
        public async Task CanGoToTheSameVmMultipleTimesAndStillGoBack()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm4.Object);

            await navigationService.GoBack(4);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoFrom(vm2, Times.Once);
            testbed.VerifyDidGoTo(vm3, Times.Exactly(3));
            testbed.VerifyDidGoFrom(vm3, Times.Exactly(3));
            testbed.VerifyDidGoTo(vm4, Times.Once);
            testbed.VerifyDidGoBackTo(vm4, Times.Never);
            testbed.VerifyDidGoBackTo(vm3, Times.Exactly(3));
            testbed.VerifyDidGoBackTo(vm2, Times.Once);
            testbed.VerifyDidGoBackTo(vm1, Times.Never);
        }

        [Fact]
        public async Task CanGoBackToRoot()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoBackToRoot();

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoBackFrom(vm2, Times.Once);
            testbed.VerifyDidGoBackTo(vm1, Times.Once);
        }

        [Fact]
        public async Task CanGoBackToRootTwice()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoBackToRoot();
            await navigationService.GoBackToRoot();

            testbed.VerifyDidGoBackTo(vm1, Times.Once); // going to root when already there should have no effect
        }

        [Fact]
        public async Task CanOpenModal()
        {
            var vm1 = testbed.CreateViewModelMock();
            await navigationService.OpenModal(vm1.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoFrom(vm1, Times.Never);
        }

        [Fact]
        public async Task CanRestartAtAPreviouslyVisitedVievModel()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);
            await navigationService.GoTo(vm4.Object);

            await navigationService.RestartAt(vm2.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Exactly(2));
            testbed.VerifyDidGoTo(vm3, Times.Once);
            testbed.VerifyDidGoTo(vm4, Times.Once);
        }

        [Fact]
        public async Task CanRestartAtAPreviouslyUnknownVievModel()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();
            var vm4 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);

            await navigationService.RestartAt(vm4.Object);

            testbed.VerifyDidGoTo(vm1, Times.Once);
            testbed.VerifyDidGoTo(vm2, Times.Once);
            testbed.VerifyDidGoTo(vm3, Times.Once);
            testbed.VerifyDidGoTo(vm4, Times.Once);
        }

        [Fact]
        public async Task CanGoBackExactlyOnceImmediatelyAfterRestartAt()
        {
            var vm1 = testbed.CreateViewModelMock();
            var vm2 = testbed.CreateViewModelMock();
            var vm3 = testbed.CreateViewModelMock();

            await navigationService.GoTo(vm1.Object);
            await navigationService.GoTo(vm2.Object);
            await navigationService.GoTo(vm3.Object);

            await navigationService.RestartAt(vm2.Object);

            await navigationService.GoBack();

            (await navigationService
                .Awaiting(async n => await n.GoBack())
                .Should()
                .ThrowAsync<NavigationException>())
                .WithMessage("Cannot navigate back");
        }
    }
}
