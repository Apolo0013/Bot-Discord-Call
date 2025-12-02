namespace Bot.Utils
{
    class UtilsFN
    {
        public static string FormatTime(ulong ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            //string final
            string msgfinal = "";
            //Horas, Minutos e Segundos
            int horas = t.Hours;
            int minutos = t.Minutes;
            int segundos = t.Seconds;
            //Tratamentos...
            if (horas > 0) msgfinal += (horas <= 9 ? "0" + horas.ToString() : horas) + ":";
            if (minutos > 0 || horas > 0) msgfinal += (minutos <= 9 ? "0" + minutos.ToString() : minutos) + ":";
            if (segundos > 0 || minutos > 0 || horas > 0) msgfinal += segundos <= 9 ? "0" + segundos.ToString() : segundos;
            //Caso especial, se somente o segundos estive sozin pae
            if (minutos == 0 && horas == 0) msgfinal += "s";
            //retornando a string ja tratada
            return msgfinal;
        }
    }
}