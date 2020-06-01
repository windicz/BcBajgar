using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WpfApp1;

namespace UnitTest.Tests
{
    [TestClass]
    public class UnitTestKontrolaDat
    {
        MainWindow obj;

        [TestMethod]
        public void PokudJsouDataSprávné()
        {
            TeplotaConvertor tmp = new TeplotaConvertor();
            Assert.AreEqual(20,tmp.Convertor("20"));
        }
        [TestMethod]
        public void PokudNejsouStejné()
        {
            TeplotaConvertor tmp = new TeplotaConvertor();
            Assert.AreNotEqual(200, tmp.Convertor("20"));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Vyjimka Vyhozena")]
        public void PokudSeVyhodiVyjimka()
        {
            TeplotaConvertor tmp = new TeplotaConvertor();
            tmp.Convertor("chyba");
        }
        [TestMethod]
        public void VyhozeniVyjimkyPomociMock()
        {
            obj = new MainWindow();
            var sender = new Mock<object>();
            var args = new Mock<RoutedEventArgs>();
            Assert.ThrowsException<Exception>(() => obj.ZvolitTeplotu(sender.Object, args.Object));
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Vyjimka Vyhozena")]
        public void VyhozeniVyjimky()
        {
            obj = new MainWindow();
            obj.ZapnutiTopeniVDatabazi();
        }
    }
}
