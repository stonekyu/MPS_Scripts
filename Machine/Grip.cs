using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : MonoBehaviour
{
    Transform putPos;
    Work serchedWork;
    Work grapWork;
    
    /// <summary>
    /// ��ũ ����
    /// </summary>
    public void Pick()
    {
        if (serchedWork != null)
        {
            //��ü �ν� �� ����ȿ�� ����
            if (serchedWork.TryGetComponent(out BoxCollider col)) col.enabled = false;
            if (serchedWork.TryGetComponent(out Rigidbody rig)) rig.isKinematic = true;
            //���� + ��ġ ����
            serchedWork.transform.SetParent(transform);
            serchedWork.transform.localPosition = Vector3.zero;
            serchedWork.transform.localRotation = Quaternion.identity;
            //���� �ִ� ���� �Ľ�
            grapWork = serchedWork;
            serchedWork = null;
        }
    }

    /// <summary>
    /// ��ũ ���� [�ν� ���]
    /// </summary>
    /// <returns></returns>
    public IEnumerator PickCo()
    {
        //��ü �ν� ������ ���
        while (serchedWork == null)
        {
            yield return null;
        }

        //��ü �ν� �� ����ȿ�� ����
        if (serchedWork.TryGetComponent(out BoxCollider col)) col.enabled = false;
        if (serchedWork.TryGetComponent(out Rigidbody rig)) rig.isKinematic = true;
        //���� + ��ġ ����
        serchedWork.transform.SetParent(transform);
        serchedWork.transform.localPosition = Vector3.zero;
        serchedWork.transform.localRotation = Quaternion.identity;
        //���� �ִ� ���� �Ľ�
        grapWork = serchedWork;
        serchedWork = null;
    }

    /// <summary>
    /// ��ü ����
    /// </summary>
    public void Place()
    {
        if (grapWork != null && putPos != null)
        {
            //���� ��ġ�� ��ü ����
            if (grapWork.TryGetComponent(out BoxCollider col)) col.enabled = true;
            grapWork.transform.SetParent(putPos);
            grapWork.transform.localPosition = Vector3.zero;
            grapWork.transform.localRotation = Quaternion.identity;
            grapWork = null;
        }
        else if (grapWork != null)
        {
            // ���� ��ġ�� �ν����� �� �ϴ� ��� ����߸���
            if (grapWork.TryGetComponent(out BoxCollider col)) col.enabled = true;
            if (grapWork.TryGetComponent(out Rigidbody rig)) rig.isKinematic = false;
            grapWork.transform.SetParent(null);
            grapWork = null;
        }
    }

    /// <summary>
    /// ��ũ ���� [�ν� ���]
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlaceCo()
    {
        if (grapWork == null) yield break;
        //���� ��ġ �ν� ������ ���
        while (putPos == null)
        {
            yield return null;
        }
        //��ü �ν� �� ����ȿ�� ����
        if (grapWork.TryGetComponent(out BoxCollider col)) col.enabled = true;
        //���� + ��ġ ����
        grapWork.transform.SetParent(putPos);
        grapWork.transform.localPosition = Vector3.zero;
        grapWork.transform.localRotation = Quaternion.identity;
        grapWork = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        //��ũ �ν� �Ľ�
        if (other.CompareTag("Work") && other.TryGetComponent(out Work work))
        {
            this.serchedWork = work;
        }
        //���� ��ġ �ν� �Ľ�
        if (other.CompareTag("Putpos"))
        {
            putPos = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //��ũ ����
        if (other.CompareTag("Work") && other.TryGetComponent(out Work work))
        {
            this.serchedWork = null;
        }
        //���� ��ġ ����
        if (other.CompareTag("Putpos"))
        {
            putPos = null;
        }
    }
}
