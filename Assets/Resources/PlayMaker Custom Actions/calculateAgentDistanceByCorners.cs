// Custom Action by DumbGameDev
// www.dumbgamedev.com

using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Path")]
    [Tooltip("Calculate the distance between the a nav mesh agent and its path destination using corners.")]

	public class calculateAgentDistanceByCorners : FsmStateAction
	{

        [RequiredField]
		[CheckForComponent(typeof(NavMeshAgent))]
		[Tooltip("Nav agent is required.")]
		public FsmOwnerDefault gameObject;

        [Title ("Destination Distance")]
        [Tooltip("The distance to the nav mesh agent path destination calculated by corners.")]
		public FsmFloat pathLengthFSM;

        public FsmBool everyFrame;

		private NavMeshAgent agent;
  
        public override void Reset()
		{

			pathLengthFSM = null;
			gameObject = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			agent = go.GetComponent<NavMeshAgent>();

			if (!everyFrame.Value)
			{
				cornersCalculation();
				Finish();
			}

		}

        public override void OnUpdate()
		{
			if (everyFrame.Value)
			{
                cornersCalculation();
			}
		}

		void cornersCalculation()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			NavMeshPath path = agent.path;
			if (path.corners.Length < 2)
				return;

			Vector3 previousCorner = path.corners[0];
			float lengthSoFar = 0.0F;
			int i = 1;
			while (i < path.corners.Length) {
				Vector3 currentCorner = path.corners[i];
				lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
				previousCorner = currentCorner;
				i++;
			}

			pathLengthFSM.Value = lengthSoFar;
            
        }

	}
}