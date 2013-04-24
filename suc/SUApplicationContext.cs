using MovablePython;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace suc
{
internal class SUApplicationContext: ApplicationContext
{
    private Hotkey hk;
    private Form form;
    private const int SWITCH_USER_COMMAND = 193;
    internal SUApplicationContext()
    {
        // только создаем форму, она все равно нужна
        // чтобы слушать хоткеи
        form = new Form();

        // создаем и регистрируем глобайльный хоткей
        hk = new Hotkey(Keys.A, false, false, false, true);
        hk.Pressed += delegate { SendSwitchCommand(); };
        if (hk.GetCanRegister(form))
            hk.Register(form);

        // Вешаем событие на выход
        Application.ApplicationExit += Application_ApplicationExit;
    }

    private void SendSwitchCommand()
    {
        // Описываем нашу службу
        ServiceController sc = new ServiceController("Sus");
        try
        {
            // посылаем ей команду
            sc.ExecuteCommand(SWITCH_USER_COMMAND);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    void Application_ApplicationExit(object sender, EventArgs e)
    {
        // при выходе разрегистрируем хоткей 
        if (hk.Registered)
            hk.Unregister();
    }
}
}
