using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Teigha.Core;
using Teigha.TD;

namespace OdaDwgAppMgd
{
    public class CmdDbReactorClass : OdRxClass
  {
    public CmdDbReactorClass(){}
    public override OdRxObject create()
    {
      return new CmdDbReactorClass();
    }
    public override String appName() { return "OdaDwgAppSwigMgd"; }
    public override String dxfName() { return ""; }
    public override String name() { return "CmdDbReactor"; }
    public override DwgVersion getClassVersion(out Teigha.Core.MaintReleaseVer pMaintReleaseVer)
    {
      pMaintReleaseVer = 0;
      return DwgVersion.vAC24;
    }
    public override DwgVersion getClassVersion() { return DwgVersion.vAC24; }
    public override UInt32 proxyFlags() { return 0; }
    public override bool isDerivedFrom(OdRxClass pClass)
    {
      if (pClass == this)
        return true;
      return OdDbDatabaseReactor.desc().isDerivedFrom(pClass);
    }
    public override OdRxClass myParent() { return OdCrypt.desc(); }
  }
    public class CmdDbReactor : OdDbDatabaseReactor
    {
        // standard part OdRxClass-related
        static CmdDbReactorClass g_pDesc = new CmdDbReactorClass();
        public static new OdRxClass desc() { return g_pDesc; }
        public override OdRxClass isA() { return g_pDesc; }
        public override OdRxObject queryX(OdRxClass pClass)
        {
          if (desc().isDerivedFrom(pClass))
            return this;
          else
            return null;
        }

        // fields
        private OdDbCommandContext m_Ctx = null;
        private bool m_bModified = false;

        public CmdDbReactor(OdDbCommandContext pCtx)
        {
            m_Ctx = pCtx;
            m_bModified = false;
            m_Ctx.database().addReactor(this);
        }
        public override void Dispose()
        {
            if (!m_bModified)
            {
                m_Ctx.database().removeReactor(this);
            }
            base.Dispose();
        }
        public void setUserIOString(String str)
        {
            m_Ctx.userIO().putString(str);
        }
        public OdDbCommandContext getCtx()
        {
            return m_Ctx;
        }

        //private section
        private void setModified()
        {
            m_bModified = true;
            m_Ctx.database().removeReactor(this); // here we should inherit OdDbDatabaseReactor to match the argument
        }

        // OdDbDatabaseReactor part
        // methods, that may be overridden

        //public override OdRxObject  clone(){return base.clone();}
        //public override void  copyFrom(OdRxObject pSource){base.copyFrom(pSource);}
        //public override void  Dispose(){base.Dispose();}
        //public override bool  Equals(object obj){return base.Equals(obj);}
        //public override int  GetHashCode(){return base.GetHashCode();}
        //public override void  goodbye(OdDbDatabase pDb){base.goodbye(pDb);}
        //public override void  headerSysVarChanged(OdDbDatabase pDb, string name){base.headerSysVarChanged(pDb, name);}
        //public override void  headerSysVarWillChange(OdDbDatabase pDb, string name){base.headerSysVarWillChange(pDb, name);}
        public override void headerSysVarWillChange(OdDbDatabase db, String str)
        {
            setModified();
        }

        //public override void  objectAppended(OdDbDatabase pDb, OdDbObject pObject){base.objectAppended(pDb, pObject);}
        //public override void  objectErased(OdDbDatabase pDb, OdDbObject pObject){base.objectErased(pDb, pObject);}
        //public override void  objectErased(OdDbDatabase pDb, OdDbObject pObject, bool erased){base.objectErased(pDb, pObject, erased);}
        //public override void  objectModified(OdDbDatabase pDb, OdDbObject pObject){base.objectModified(pDb, pObject);}
        //public override void  objectOpenedForModify(OdDbDatabase pDb, OdDbObject pObject){base.objectForModify(pDb, pObject);}
        public override void objectOpenedForModify(OdDbDatabase db, OdDbObject obj)
        {
            setModified();
        }
        //public override void  objectReAppended(OdDbDatabase pDb, OdDbObject pObject){base.objectReAppended(pDb, pObject);}
        //public override void  objectUnAppended(OdDbDatabase pDb, OdDbObject pObject){base.objectUnAppended(pDb, pObject);}
        //public override void  proxyResurrectionCompleted(OdDbDatabase pDb, string appname, OdDbObjectIdArray objectIds){base.proxyResurrectionCompleted(pDb. appname, objectIds);}

        // public methods
        public bool isDatabaseModified() { return m_bModified; }
    }

    public class CmdReactor : OdEdCommandStackReactor
    {
        // variables
        private String            m_sLastInput = String.Empty;

        // special variable CmdDbReactor
        // as multiple inheritance is impossible we provide a special variable derived from OdDbDatabaseReactor to override its methods if necessary
        private CmdDbReactor dbReactor = null;

        private Form1 appForm = null;

        public void setForm(Form1 Frm) {appForm = Frm;}
        // private section
        private void undoCmd()
        {
            OdDbDatabase pDb = dbReactor.getCtx().database();
            try
            {
                pDb.disableUndoRecording(true);
                pDb.undo();
                pDb.disableUndoRecording(false);
            }
            catch (OdError err)
            {
                throw new Exception("Can't repair database: " + err.Message);
            }
        }

        // public section
        public CmdReactor(OdDbCommandContext pCmdCtx) 
        {
            dbReactor = new CmdDbReactor(pCmdCtx);
            Globals.odedRegCmds().addReactor(this);
        }
        ~CmdReactor()
        {
            dbReactor.Dispose();
            Globals.odedRegCmds().removeReactor(this);
        }
        public void setLastInput(String sLastInput)
        {
            m_sLastInput = sLastInput;
        }
        public String lastInput()
        {
            return m_sLastInput;
        }

        public bool isDatabaseModified() { return dbReactor.isDatabaseModified(); }


        public override OdEdCommand unknownCommand(String sCmdName, OdEdCommandContext pCmdCtx)
        {
            String sMsg = String.Format("Unknown command {0}.", sCmdName);
            dbReactor.setUserIOString(sMsg); //m_Ctx.userIO().putString(sMsg);            
            return new OdEdCommand();
        }
        public override void commandWillStart(OdEdCommand pCmd, OdEdCommandContext pCmdCtx)
        {
            String lastInput = m_sLastInput.ToUpper();
            if ((pCmd.flags() & OdEdCommand.kNoHistory) != 0)
            {
                appForm.setRecentCmdName(m_sLastInput);
            }
            

            if ((pCmd.flags() & OdEdCommand.kNoUndoMarker) != 0)
            {
                dbReactor.getCtx().database().startUndoRecord();
            }
        }
        public override void commandCancelled(OdEdCommand pCmd, OdEdCommandContext pCmdCtx)
        {
            undoCmd();
        }
        public override void commandFailed(OdEdCommand pCmd, OdEdCommandContext pCmdCtx)
        {
            undoCmd();
        }
    }
}
