using UnityEngine;
using Emesefe.Utilities;
using System.Collections.Generic;

namespace TaskSystem {

    public class GameManager : MonoBehaviour {

        private TaskSystem<Task> taskSystem;
        public static TaskSystem<TransporterTask> transporterTaskSystem;
        
        private List<WeaponSlot> weaponSlotList;
        
        [SerializeField] private GameObject workerPrefab;
        [SerializeField] private GameObject stainPrefab;
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private GameObject weaponSlotPrefab;

        private void Start() {
            taskSystem = new TaskSystem<Task>();
            transporterTaskSystem = new TaskSystem<TransporterTask>();
            weaponSlotList = new List<WeaponSlot>();

            Worker worker = InstantiateWorker(Vector3.right * 10);
            WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
            workerTaskAI.Setup(worker, taskSystem);
            
            worker = InstantiateWorker(Vector3.left * 10);
            WorkerTransporterTaskAI workerTransporterTaskAI = worker.gameObject.AddComponent<WorkerTransporterTaskAI>();
            workerTransporterTaskAI.Setup(worker, transporterTaskSystem);
            
            GameObject weaponSlotGameObject = InstantiateWeaponSlot(Vector3.zero);
            weaponSlotList.Add(new WeaponSlot(weaponSlotGameObject.transform));
            
            weaponSlotGameObject = InstantiateWeaponSlot(Vector3.up * 5);
            weaponSlotList.Add(new WeaponSlot(weaponSlotGameObject.transform));
            
            weaponSlotGameObject = InstantiateWeaponSlot(Vector3.down * 5);
            weaponSlotList.Add(new WeaponSlot(weaponSlotGameObject.transform));
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject weaponGameObject = InstantiateWeapon(Utils.GetMouseWorldPosition());
                taskSystem.EnqueueTask(() =>
                {
                    foreach (WeaponSlot weaponSlot in weaponSlotList)
                    {
                        if (weaponSlot.IsEmpty())
                        {
                            weaponSlot.SetHasWeaponIncoming(true);
                            Task task = new Task.TakeWeaponToWeaponSlot
                            {
                                weaponPosition = weaponGameObject.transform.position,
                                weaponSlotPosition = weaponSlot.GetPosition(),
                                grabWeapon = (workerTaskAI) =>
                                {
                                    weaponGameObject.transform.SetParent(workerTaskAI.transform);
                                },
                                dropWeapon = () =>
                                {
                                    weaponGameObject.transform.SetParent(null);
                                    weaponSlot.SetWeaponTransform(weaponGameObject.transform);
                                }
                            };

                            return task;
                        }
                        // WeaponSlot not empty, keep looking
                    }
                    
                    // No weaponSlot empty, try again later
                    return null;
                });
            }
        }

        private Worker InstantiateWorker(Vector3 position)
        {
            return Instantiate(workerPrefab, position, Quaternion.identity).GetComponent<Worker>();
        }
        
        private GameObject InstantiateStain(Vector3 position)
        {
            return Instantiate(stainPrefab, position, Quaternion.identity);
        }

        private void InstantiateStainWithTask(Vector3 position)
        {
            GameObject stain = InstantiateStain(position);
            float cleanUpTime = Time.time + 5f; // 5 seconds must pass after instantiation until start cleaning
            
            taskSystem.EnqueueTask(() =>
            {
                if (Time.time >= cleanUpTime)
                {
                    Task newTask = new Task.CleanUpTask
                    {
                        targetPosition = stain.transform.position,
                    
                        onCleanupAction = () =>
                        {
                            float alpha = 1;
                            FunctionUpdater.Create(() =>
                            {
                                alpha -= Time.deltaTime;
                                foreach (Transform child in stain.transform)
                                {
                                    SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                                    Color color = spriteRenderer.color;
                                    spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
                                }

                            });
                        }
                    };
                    return newTask;
                }

                return null;
            });
        }
        
        private GameObject InstantiateWeapon(Vector3 position)
        {
            return Instantiate(weaponPrefab, position, Quaternion.identity);
        }
        
        private GameObject InstantiateWeaponSlot(Vector3 position)
        {
            return Instantiate(weaponSlotPrefab, position, Quaternion.identity);
        }
    }
}