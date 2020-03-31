using System;
using System.Data;

namespace ADUserUpdater.Service
{
    class DataModule
    {
        private readonly PROSql.SQL sql;
        private readonly Logger logger;

        private readonly bool debug = true;

        public DataModule(Logger _logger)
        {
            logger = _logger;

            sql = new PROSql.SQL(Properties.Settings.Default.SQLServerName,
                                Properties.Settings.Default.SQLDatabase,
                                Properties.Settings.Default.SQLUsername,
                                Properties.Settings.Default.SQLPassword,
                                Properties.Settings.Default.SQLNT);
        }

        private void SqlConnect()
        {
            if (sql.Connect())
                logger.Log("Nawiązano połączenie z serwerem SQL", 0);
        }

        private void SqlDisconnect()
        {
            sql.Disconnect();
            logger.Log("Zamknięto połączenie z serwerem SQL", 0);
        }

        public void Dispose()
        {
            sql.Disconnect();
            logger.Log("Zamknięto połączenie z serwerem SQL", 0);
        }

        public DataTable GetADInfo()
        {
            if (debug)
                return GetADInfoMock();

            DataTable dt;
            try
            {
                SqlConnect();
                dt = sql.Execute("exec CDN.PROGetADInfo");
                logger.Log("Pobrano liste ADInfo w ilości [" + dt.Rows.Count + "]", 0);
            }
            catch (Exception e)
            {
                Dispose();
                throw new Exception("Błąd przy pobieraniu listy ADInfo: " + e.Message);
            }
            return dt;
        }

        private DataTable GetADInfoMock()
        {
            DataTable dt;
            try
            {
                SqlConnect();
                dt = sql.Execute("select 'Łukasz1' as Imie, 'Rędziński1' as Nazwisko, 'StanPL' as StanowiskoPL, 'StanEN' as StanowiskoEN, 'l.redzinski@kgl.pl' as Email, '123321' as TelKom, 'w.golabkksk@kgl.pl' as  ManagerEmail union select 'Łukasz1' as Imie, 'Rędziński1' as Nazwisko, 'StanPL' as StanowiskoPL, 'StanEN' as StanowiskoEN, 'l.redzinski@kgl.pl' as Email, '123321' as TelKom, 'w.golabb@kgl.pl' as  ManagerEmail");
                logger.Log("Pobrano liste ADInfo w ilości [" + dt.Rows.Count + "]", 0);
            }
            catch (Exception e)
            {
                Dispose();
                throw new Exception("Błąd przy pobieraniu listy ADInfo: " + e.Message);
            }
            return dt;
        }
    }
}
