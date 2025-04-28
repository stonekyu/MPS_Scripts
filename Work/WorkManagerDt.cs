using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManagerDt : MonoBehaviour
{
    const int poolCnt = 50;
    List<GameObject> worklist;
    Queue<GameObject> workPool;
    [SerializeField] GameObject workPrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] List<Goal> goals;

    private void Awake()
    {
        PoolingObject();

        foreach (Goal goal in goals)
        {
            goal.onFinishWork += (obj) => { DestroyWork(obj); };
        }
    }

    /// <summary>
    /// 워크 오브젝트 풀링
    /// </summary>
    private void PoolingObject()
    {
        workPool = new Queue<GameObject>();
        worklist = new List<GameObject>();

        //일정 갯수만큼 생성 후 비활성화
        for (int i = 0; i < poolCnt; i++)
        {
            GameObject work = Instantiate(workPrefab, spawnPos.position, spawnPos.rotation, spawnPos);
            workPool.Enqueue(work);
            worklist.Add(work);
            work.SetActive(false);
        }
    }

    /// <summary>
    /// 객체 생성
    /// 미리 생성해 놓은 오브젝트 중 한개를 빼서 활성화
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnWork()
    {
        if (CheckWork())
        {
            return null;
        }

        GameObject obj = workPool.Dequeue();
        obj.transform.SetParent(spawnPos);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.SetActive(true);

        if (obj.TryGetComponent(out BoxCollider col)) col.enabled = true;
        if (obj.TryGetComponent(out Rigidbody rig)) rig.isKinematic = false;

        return obj;
    }

    /// <summary>
    /// 객체 삭제
    /// 객체를 비활성화해서 대기시킴
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyWork(GameObject obj)
    {
        if (obj.TryGetComponent<Work>(out Work work))
        {
            work.WorkType = State.WorkType.None;
            work.gameObject.SetActive(false);
            work.transform.SetParent(spawnPos);
            work.transform.localPosition = Vector3.zero;
            work.transform.localRotation = Quaternion.identity;
            workPool.Enqueue(work.gameObject);
        }
        else
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// 해당 위치에 워크가 있는지 체크
    /// </summary>
    /// <returns></returns>
    public bool CheckWork()
    {
        Ray ray = new Ray(spawnPos.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.0f))
        {
            if (hit.collider.CompareTag("Work"))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 워크 초기화
    /// 미리 생성해 놓았던 워크들 전체 비활성화
    /// </summary>
    public void ResetWorks()
    {
        foreach (GameObject work in worklist)
        {
            DestroyWork(work);
        }
    }
}
