using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : MonoBehaviour
{
    Transform putPos;
    Work serchedWork;
    Work grapWork;
    
    /// <summary>
    /// 워크 집기
    /// </summary>
    public void Pick()
    {
        if (serchedWork != null)
        {
            //물체 인식 및 물리효과 해제
            if (serchedWork.TryGetComponent(out BoxCollider col)) col.enabled = false;
            if (serchedWork.TryGetComponent(out Rigidbody rig)) rig.isKinematic = true;
            //집기 + 위치 조정
            serchedWork.transform.SetParent(transform);
            serchedWork.transform.localPosition = Vector3.zero;
            serchedWork.transform.localRotation = Quaternion.identity;
            //집고 있는 자재 파싱
            grapWork = serchedWork;
            serchedWork = null;
        }
    }

    /// <summary>
    /// 워크 집기 [인식 대기]
    /// </summary>
    /// <returns></returns>
    public IEnumerator PickCo()
    {
        //물체 인식 전까지 대기
        while (serchedWork == null)
        {
            yield return null;
        }

        //물체 인식 및 물리효과 해제
        if (serchedWork.TryGetComponent(out BoxCollider col)) col.enabled = false;
        if (serchedWork.TryGetComponent(out Rigidbody rig)) rig.isKinematic = true;
        //집기 + 위치 조정
        serchedWork.transform.SetParent(transform);
        serchedWork.transform.localPosition = Vector3.zero;
        serchedWork.transform.localRotation = Quaternion.identity;
        //집고 있는 자재 파싱
        grapWork = serchedWork;
        serchedWork = null;
    }

    /// <summary>
    /// 물체 놓기
    /// </summary>
    public void Place()
    {
        if (grapWork != null && putPos != null)
        {
            //놓을 위치로 물체 놓기
            if (grapWork.TryGetComponent(out BoxCollider col)) col.enabled = true;
            grapWork.transform.SetParent(putPos);
            grapWork.transform.localPosition = Vector3.zero;
            grapWork.transform.localRotation = Quaternion.identity;
            grapWork = null;
        }
        else if (grapWork != null)
        {
            // 놓을 위치를 인식하지 못 하는 경우 떨어뜨리기
            if (grapWork.TryGetComponent(out BoxCollider col)) col.enabled = true;
            if (grapWork.TryGetComponent(out Rigidbody rig)) rig.isKinematic = false;
            grapWork.transform.SetParent(null);
            grapWork = null;
        }
    }

    /// <summary>
    /// 워크 집기 [인식 대기]
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlaceCo()
    {
        if (grapWork == null) yield break;
        //놓을 위치 인식 전까지 대기
        while (putPos == null)
        {
            yield return null;
        }
        //물체 인식 및 물리효과 해제
        if (grapWork.TryGetComponent(out BoxCollider col)) col.enabled = true;
        //놓기 + 위치 조정
        grapWork.transform.SetParent(putPos);
        grapWork.transform.localPosition = Vector3.zero;
        grapWork.transform.localRotation = Quaternion.identity;
        grapWork = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        //워크 인식 파싱
        if (other.CompareTag("Work") && other.TryGetComponent(out Work work))
        {
            this.serchedWork = work;
        }
        //놓을 위치 인식 파싱
        if (other.CompareTag("Putpos"))
        {
            putPos = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //워크 해제
        if (other.CompareTag("Work") && other.TryGetComponent(out Work work))
        {
            this.serchedWork = null;
        }
        //놓을 위치 해제
        if (other.CompareTag("Putpos"))
        {
            putPos = null;
        }
    }
}
