using VailderChessDesktop.Network.Core;

namespace TestProject;

public class NetworkCoreUtilityTest
{
    [Test]
    public void IsCorrectServerAddress()
    {
        //Корректный локальный адрес сервера и порт сервера
        Assert.That(Utility.IsCorrectServerAddress("127.0.0.1", "8015"), Is.True);
        Assert.That(Utility.IsCorrectServerAddress("127.000.000.001", "8000"), Is.True);
        Assert.That(Utility.IsCorrectServerAddress("192.168.0.105", "8500"), Is.True);
        Assert.That(Utility.IsCorrectServerAddress("127.5.00.01", "8435"), Is.True);
        Assert.That(Utility.IsCorrectServerAddress("011.105.00.6", "55234"), Is.True);
        
        //Некорректный локальный адрес сервера
        Assert.That(Utility.IsCorrectServerAddress("625.0.0.1", "8015"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress("1270.0.0.1", "8000"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress("192.0.105", "8500"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress("127.0.0.1.", "8000"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress(".127.0.0.1", "8000"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress("", "8000"), Is.False);
        
        //Некорректный порт сервера
        Assert.That(Utility.IsCorrectServerAddress("127.0.0.1", "-0001"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress("127.0.0.1", "70000"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress("127.0.0.1", "8.00"), Is.False);
        Assert.That(Utility.IsCorrectServerAddress("127.0.0.1", ""), Is.False);
    }
}

