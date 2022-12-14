// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Finds the Child of a GameObject by Name and/or Tag. Sends events based on result ( Found or not found). NOTE: This action will search recursively through all children and return the first match; To find a specific child use Find Child.")]
	public class HasChild : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject to search.")]
		public FsmOwnerDefault gameObject;

        [Tooltip("The name of the child to search for.")]
		public FsmString childName;
		
		[UIHint(UIHint.Tag)]
        [Tooltip("The Tag to search for. If Child Name is set, both name and Tag need to match.")]
		public FsmString withTag;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a GameObject variable.")]
		public FsmGameObject storeResult;
		
		[UIHint(UIHint.Variable)]
        [Tooltip("True if child was found")]
		public FsmBool found;
		
		public FsmEvent foundEvent;
		
		public FsmEvent notFoundEvent;

		public override void Reset()
		{
			gameObject = null;
			childName = "";
			withTag = "Untagged";
			storeResult = null;
			
			found = null;
			
			foundEvent = null;
			
			notFoundEvent = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = DoGetChildByName(Fsm.GetOwnerDefaultTarget(gameObject), childName.Value, withTag.Value);
			
		
			if (storeResult.Value != null)
			{
				found.Value = true;
				Fsm.Event(foundEvent);
			}else{
				found.Value = false;
				Fsm.Event(notFoundEvent);
			}
			
			Finish();
		}

		static GameObject DoGetChildByName(GameObject root, string name, string tag)
		{
			if (root == null)
			{
				return null;
			}

			foreach (Transform child in root.transform)
			{
				if (!string.IsNullOrEmpty(name))
				{
					if (child.name == name)
					{
						if (!string.IsNullOrEmpty(tag))
						{
							if (child.tag.Equals(tag))
							{
								return child.gameObject;
							}
						}
						else
						{
							return child.gameObject;
						}
					}
				}
				else if (!string.IsNullOrEmpty((tag)))
				{
					if (child.tag == tag)
					{
						return child.gameObject;
					}
				}

				// search recursively

				var returnObject = DoGetChildByName(child.gameObject, name, tag);
				if(returnObject != null)
				{
					return returnObject;
				}
			}

			return null;
		}

		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(childName.Value) && string.IsNullOrEmpty(withTag.Value))
			{
				return "Specify Child Name, Tag, or both.";
			}
			return null;
		}

	}
}
