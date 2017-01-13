/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace HoloToolkit.Sharing {

public class RoomManager : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal RoomManager(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(RoomManager obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~RoomManager() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          SharingClientPINVOKE.delete_RoomManager(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public virtual void AddListener(RoomManagerListener newListener) {
    SharingClientPINVOKE.RoomManager_AddListener(swigCPtr, RoomManagerListener.getCPtr(newListener));
  }

  public virtual void RemoveListener(RoomManagerListener oldListener) {
    SharingClientPINVOKE.RoomManager_RemoveListener(swigCPtr, RoomManagerListener.getCPtr(oldListener));
  }

  public virtual int GetRoomCount() {
    int ret = SharingClientPINVOKE.RoomManager_GetRoomCount(swigCPtr);
    return ret;
  }

  public virtual Room GetRoom(int index) {
    global::System.IntPtr cPtr = SharingClientPINVOKE.RoomManager_GetRoom(swigCPtr, index);
    Room ret = (cPtr == global::System.IntPtr.Zero) ? null : new Room(cPtr, true);
    return ret; 
  }

  public virtual Room GetCurrentRoom() {
    global::System.IntPtr cPtr = SharingClientPINVOKE.RoomManager_GetCurrentRoom(swigCPtr);
    Room ret = (cPtr == global::System.IntPtr.Zero) ? null : new Room(cPtr, true);
    return ret; 
  }

  public virtual Room CreateRoom(XString roomName, long roomID, bool keepOpenWhenEmpty) {
    global::System.IntPtr cPtr = SharingClientPINVOKE.RoomManager_CreateRoom(swigCPtr, XString.getCPtr(roomName), roomID, keepOpenWhenEmpty);
    Room ret = (cPtr == global::System.IntPtr.Zero) ? null : new Room(cPtr, true);
    return ret; 
  }

  public virtual bool JoinRoom(Room room) {
    bool ret = SharingClientPINVOKE.RoomManager_JoinRoom(swigCPtr, Room.getCPtr(room));
    return ret;
  }

  public virtual bool LeaveRoom() {
    bool ret = SharingClientPINVOKE.RoomManager_LeaveRoom(swigCPtr);
    return ret;
  }

  public virtual bool DownloadAnchor(Room room, XString anchorName) {
    bool ret = SharingClientPINVOKE.RoomManager_DownloadAnchor(swigCPtr, Room.getCPtr(room), XString.getCPtr(anchorName));
    return ret;
  }

  public unsafe bool UploadAnchor(Room room, XString anchorName, byte[] data, int dataSize) {
    fixed ( byte* swig_ptrTo_data = data ) {
    {
      bool ret = SharingClientPINVOKE.RoomManager_UploadAnchor(swigCPtr, Room.getCPtr(room), XString.getCPtr(anchorName), (global::System.IntPtr)swig_ptrTo_data, dataSize);
      return ret;
    }
    }
  }

}

}
