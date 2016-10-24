using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDraggablePanel2 : UIDraggablePanel
{
	//=====================================================================
	//
	// Fields & Properties - UI
	//
	//=====================================================================
	
	/// <summary>
	/// UIGrdi - 아이템 부모
	/// </summary>
	public UIGrid Grid;
	
	/// <summary>
	/// 프리팹
	/// </summary>
	public GameObject TemplatePrefab;
	
	//=====================================================================
	//
	// Fields - Variable
	//
	//=====================================================================
	
	/// <summary>
	/// 총 아이템의 개수
	/// </summary>
	private int ItemCount;
	
	/// <summary>
	/// 첫 부분과 끝 부분의 아이템. grid 영역의 크기를 구하기 위함.
	/// </summary>
	private Transform mFirstTemplate = null;
	private Transform mLastTemplate = null;
	
	/// <summary>
	/// 첫 부분과 끝 부분의 위치
	/// </summary>
	private Vector3 mFirstPosition = Vector3.zero;
	private Vector3 mPrevPosition = Vector3.zero;
	
	/// <summary>
	/// 관리 리스트
	/// </summary>
	private List<UIListItem> mList = new List<UIListItem>();
	
	/// <summary>
	/// 화면에 보여질 최소한의 개수 
	/// </summary>
	private int mMinShowCount;
	
	//=====================================================================
	//
	// Fields & Properties - Events
	//
	//=====================================================================
	
	public delegate void ChangeIndexDelegate(UIListItem item, int index);
	private ChangeIndexDelegate mCallback;
	
	//=====================================================================
	//
	// Fields & Properties - Get & Set
	//
	//=====================================================================
	
	/// <summary>
	/// 머리를 가리킨다.
	/// </summary>
	private UIListItem Head { get { return mList.Count <= 0 ? null : mList[0]; } }
	
	/// <summary>
	/// 꼬리를 가리킨다.
	/// </summary>
	private UIListItem Tail { get { return mList.Count <= 0 ? null : mList[mList.Count - 1]; } }
	
	/// <summary>
	/// 화면에 보일 수 있는 가로 개수
	/// </summary>
	private int maxCol { get { return Grid.PerLine; } }
	
	/// <summary>
	/// 화면에 보일 수 있는 세로 개수
	/// </summary>
	private int maxRow { get { return Mathf.CeilToInt(panel.clipRange.w / Grid.cellHeight); } }
	public  int maxScreenLine{ set {_maxScreenLine = value; }}
	private int _maxScreenLine;
	public  int maxColLine{ set {_maxColLine = value; }}
	private int _maxColLine;
	//=====================================================================
	//
	// Methods - UIDraggablePanel override
	//
	//=====================================================================
	
	/// <summary>
	/// Calculate the bounds used by the widgets.
	/// </summary>
	public override Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mBounds = CalculateRelativeWidgetBounds2(mTrans, mFirstTemplate, mLastTemplate);
			}
			return mBounds;
		}
	}
	
	public override void Awake()
	{
		base.Awake();
	}
	
	public override void Start()
	{
		base.Start();
		mFirstPosition = mTrans.localPosition;
		mPrevPosition = mTrans.localPosition;
	}

	public void reSetGridPannel(){
		RemoveAll();
		base.Awake();
		base.Start();
		mFirstPosition = mTrans.localPosition;
		mPrevPosition = mTrans.localPosition;
		mCallback = null;

	}

	public void SetInitialPosition(){
		horizontalScrollBar.scrollValue = 0.3f;
	}

	void Destory()
	{
		RemoveAll();
	}
	private int myRow;
	/// <summary>
	/// 아이템을 생성한다.
	/// </summary>
	/// <param name="count"></param>
	/// <param name="callback"></param>
	public void Init(int count, ChangeIndexDelegate callback)
	{
		mCallback = callback;
		if(mTrans == null) mTrans = transform;
		if(panel == null) mPanel = GetComponent<UIPanel>();

		ItemCount = count;
		SetTemplate(count);
		
		//RemoveAll();
		//mList.Clear();
		
		//화면에 보여질 개수
		myRow = (maxRow+_maxScreenLine);
		mMinShowCount = maxCol * myRow;
		int makeCount = Mathf.Min(count, mMinShowCount);
		
		GameObject obj = null;
		UIListItem prevItem = null;
		for (int i = 0; i < makeCount; i++)
		{

			UIListItem item = null;
			if (i >= mList.Count) //이미 생성한 list가 있다면 재사용.
			{
				obj = NGUITools.AddChild(Grid.gameObject, TemplatePrefab);
				if (obj.GetComponent<UIDragPanelContents>() == null)
					obj.AddComponent<UIDragPanelContents>().draggablePanel = this;
				item = new UIListItem();
				item.Target = obj;
				mList.Add(item);
			}
			else{
				item = mList[i];
			}
			item.SetIndex(i);
			item.Prev = prevItem;
			item.Next = null;
			if (prevItem != null)
				prevItem.Next = item;
			prevItem = item;
			
			mCallback(item, i);
		}
		for( int i=makeCount; i<mList.Count; i++ ){
			mList[i].Target.gameObject.SetActive(false);
		}

		UpdatePosition();
	}
	
	/// <summary>
	/// Restrict the panel's contents to be within the panel's bounds.
	/// </summary>
	public override bool RestrictWithinBounds(bool instant)
	{
		Vector3 constraint = panel.CalculateConstrainOffset(bounds.min, bounds.max);
		
		if (constraint.magnitude > 0.001f)
		{
			if (!instant && dragEffect == DragEffect.MomentumAndSpring)
			{
				// Spring back into place
				SpringPanel.Begin(panel.gameObject, mTrans.localPosition + constraint, 13f, UpdateCurrentPosition);
			}
			else
			{
				// Jump back into place
				//Utility.Log("x : " + constraint + " bool : "+ instant);
				MoveRelative(constraint);
				mMomentum = Vector3.zero;
				mScroll = 0f;
			}
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Changes the drag amount of the panel to the specified 0-1 range values.
	/// (0, 0) is the top-left corner, (1, 1) is the bottom-right.
	/// </summary>
	public override void SetDragAmount(float x, float y, bool updateScrollbars)
	{

		base.SetDragAmount(x, y, updateScrollbars);
		
		UpdateCurrentPosition();
	}
	
	/// <summary>
	/// Move the panel by the specified amount.
	/// </summary>
	public override void MoveRelative(Vector3 relative)
	{

		base.MoveRelative(relative);
		UpdateCurrentPosition();
	}
	
	//=====================================================================
	//
	// Methods - UIDraggablePanel2
	//
	//=====================================================================
	
	/// <summary>
	/// 꼬리부분을 머리부분으로 옮긴다. 
	/// </summary>
	public void TailToHead()
	{
		for (int i = 0; i < maxCol; i++)
		{
			UIListItem item = Tail;
			
			if (item == null)
				return;
			
			if (item == Head)
				return;
			
			if (item.Prev != null)
				item.Prev.Next = null;
			
			item.Next = Head;
			item.Prev = null;
			
			Head.Prev = item;
			
			mList.RemoveAt(mList.Count - 1);
			mList.Insert(0, item);
		}
	}
	
	/// <summary>
	/// 머리 부분을 꼬리 부분으로 옮긴다. 
	/// </summary>
	public void HeadToTail()
	{
		for (int i = 0; i < maxCol; i++)
		{
			UIListItem item = Head;
			
			if (item == null)
				return;
			
			if (item == Tail)
				return;
			
			item.Next.Prev = null;
			item.Next = null;
			item.Prev = Tail;
			
			Tail.Next = item;
			
			mList.RemoveAt(0);
			mList.Insert(mList.Count, item);
		}
	}
	
	/// <summary>
	/// 실제 아이템 양 끝쪽에 임시 아이템을 생성 후 cllipping 되는 영역의 bound를 구한다.
	/// </summary>
	/// <param name="count"></param>
	private void SetTemplate(int count)
	{
		if (mFirstTemplate == null)
		{
			GameObject firstTemplate = NGUITools.AddChild(Grid.gameObject, TemplatePrefab);
			firstTemplate.SetActive(false);
			mFirstTemplate = firstTemplate.transform;
			mFirstTemplate.name = "first rect";
		}
		
		if (mLastTemplate == null)
		{
			GameObject lastTemplate = NGUITools.AddChild(Grid.gameObject, TemplatePrefab);
			lastTemplate.SetActive(false);
			mLastTemplate = lastTemplate.transform;
			mLastTemplate.name = "last rect";
		}
		
		mFirstTemplate.localPosition = Vector3.zero; //처음위치
		if (Grid.arrangement == UIGrid.Arrangement.Vertical){
			mLastTemplate.localPosition = new Vector3( (Grid.PerLine-1) * Grid.cellWidth, 
			                                          -Grid.cellHeight * (count/Grid.PerLine), 0); //끝위치
		}else{
			mLastTemplate.localPosition = new Vector3(  Grid.cellWidth* (count/Grid.PerLine), 
			                                          -Grid.cellHeight * (Grid.PerLine-1) , 0); //끝위치
		}

		mCalculatedBounds = true;

		mBounds = CalculateRelativeWidgetBounds2(mTrans, mFirstTemplate, mLastTemplate);
		
		Vector3 constraint = panel.CalculateConstrainOffset(bounds.min, bounds.max);
		//Utility.Log("realtive" +( mTrans.localPosition + constraint));
		SpringPanel.Begin(panel.gameObject, mTrans.localPosition + constraint, 13f, UpdateCurrentPosition);
	}
	
	/// <summary>
	/// 아이템들의 재사용을 위하여 위치를 조절한다.
	/// </summary>
	public void UpdateCurrentPosition()
	{	
		Vector3 currentPos = mFirstPosition - mTrans.localPosition;
		if (Grid.arrangement == UIGrid.Arrangement.Vertical)
		{
			bool isScrollUp = currentPos.y > mPrevPosition.y;
			//if ((int)currentPos.y == (int)mPrevPosition.y) return;
			int headIndex = (int)(-currentPos.y / Grid.cellHeight) * maxCol + _maxColLine;
			headIndex = Mathf.Clamp(headIndex, 0, ItemCount-1);
			//Utility.Log(headIndex);
			if( Head.Index != headIndex && headIndex <= ItemCount - mList.Count)
			{
				//스크롤의 방향에 따라 여분의 객체 이동
				if( isScrollUp )
					TailToHead();
				else
					HeadToTail();//Utility.Log("pos ");
				//index는 head부터 순서대로
				SetIndexHeadtoTail(headIndex);
				UpdatePosition();
			}

			/*

			if (isScrollUp)
			{
				int headIndex = (int)(-currentPos.y / Grid.cellHeight) * maxCol;  //가로줄의 맨 처음 
				headIndex = Mathf.Clamp(headIndex, 0, ItemCount - 1);
				
				if (Head != null && Head.Index != headIndex && headIndex <= ItemCount - mList.Count)
				{
					// 꼬리 -> 처음.
					TailToHead();
					SetIndexHeadtoTail(headIndex);
					UpdatePosition();
				}
			}
			else
			{
				//가로줄의 맨 오른쪽
				int tailIndex = Mathf.CeilToInt((-currentPos.y + panel.clipRange.w) / Grid.cellHeight) * maxCol -1; 
				tailIndex = Mathf.Clamp(tailIndex, 0, ItemCount - 1);
				
				if (Tail != null && Tail.Index != tailIndex && tailIndex >= mList.Count)
				{
					// 처음 -> 꼬리.
					HeadToTail();
					SetIndexTailtoHead(tailIndex);
					UpdatePosition();
				}
			}*/
		}else{
			bool isScrollRight = currentPos.x < mPrevPosition.x;
			if(isScrollRight) 
			{
				int headIndex = (int)(currentPos.x / Grid.cellWidth);
				headIndex = Mathf.Clamp( headIndex , 0, ItemCount - 1 );
				if( Head != null && Head.Index != headIndex && headIndex <= ItemCount - mList.Count )
				{
					// 꼬리 -> 처음.
					TailToHead();
					SetIndexHeadtoTail( headIndex );
					UpdatePosition();
				}
			} 
			else 
			{
				int tailIndex = (int)((currentPos.x + panel.clipRange.z) / Grid.cellWidth);
				tailIndex = Mathf.Clamp( tailIndex , 0, ItemCount - 1 );
				if( Tail != null && Tail.Index != tailIndex && tailIndex >= mList.Count )
				{
					// 처음 -> 꼬리.
					HeadToTail();
					SetIndexTailtoHead( tailIndex );
					UpdatePosition();
				}
			
			}

		}
		mPrevPosition = currentPos;
	}
	
	/// <summary>
	/// head부터 index를 재 정리한다.
	/// </summary>
	/// <param name="headIndex"></param>
	public void SetIndexHeadtoTail(int headIndex)
	{
		UIListItem item = null;
		int index = -1;
		for (int i = 0; i < mList.Count; i++)
		{
			index = i + headIndex;
			item = mList[i];
			item.SetIndex(index);
			
			mCallback(item, index);
		}
	}
	
	/// <summary>
	/// tail부터 index를 재 정리한다.
	/// </summary>
	/// <param name="tailIndex"></param>
	public void SetIndexTailtoHead(int tailIndex)
	{
		UIListItem item = null;
		int index = -1;
		int cnt = mList.Count;
		for (int i = 0; i < cnt; i++)
		{
			index = tailIndex - i;
			item = mList[cnt - i - 1];
			item.SetIndex(index);
			mCallback(item, index);
		}
	}
	
	/// <summary>
	/// 아이템들의 위치를 정한다.
	/// </summary>
	private void UpdatePosition()
	{
	
		for (int i = 0; i < mList.Count; i++)
		{
			Transform t = mList[i].Target.transform;
			
			Vector3 position = Vector3.zero;
			if (Grid.arrangement == UIGrid.Arrangement.Vertical)
			{
				//index를 기준으로 위치를 다시 잡는다.
				position.x += mList[i].Index % maxCol * Grid.cellWidth;
				position.y -= mList[i].Index / maxCol * Grid.cellHeight;
				
			}
			else if (Grid.arrangement == UIGrid.Arrangement.Horizontal)
			{
			
			//	position.x += mList[i].Index % maxCol * Grid.cellWidth;
			//	position.y -= mList[i].Index / maxCol * Grid.cellHeight;

			
			}                
			t.localPosition = position;
			t.name = string.Format("item_{0}", mList[i].Index);
			//Utility.Log("name " +t.name); OnInviteFreind
		}

	}
	
	/// <summary>
	/// 해당 아이템을 삭제한다.
	/// </summary>
	/// <param name="item"></param>
	public void RemoveItem(UIListItem item)
	{
		if (item.Prev != null)
		{
			item.Prev.Next = item.Next;
		}
		
		if (item.Next != null)
		{
			item.Next.Prev = item.Prev;
		}
		
		UIListItem tmp = item.Next as UIListItem;
		int idx = item.Index;
		int tempIdx;
		while (tmp != null)
		{
			tempIdx = tmp.Index;
			tmp.Index = idx;
			mCallback(tmp, tmp.Index);
			
			idx = tempIdx;
			tmp = tmp.Next as UIListItem;
			
		}
		
		UIListItem tail = Tail;
		mList.Remove(item);
		
		if (ItemCount < mMinShowCount)
		{
			GameObject.DestroyImmediate(item.Target);
		}
		else
		{
			if (item == tail || Tail.Index >= ItemCount - 1)
			{
				// add head
				Head.Prev = item;
				item.Next = Head;
				item.Prev = null;
				item.Index = Head.Index - 1;
				mList.Insert(0, item);
				mCallback(item, item.Index);
				
				Vector3 constraint = panel.CalculateConstrainOffset(bounds.min, bounds.max);
				SpringPanel.Begin(panel.gameObject, mTrans.localPosition + constraint, 13f, UpdateCurrentPosition);
			}
			else
			{
				// add tail
				Tail.Next = item;
				item.Prev = Tail;
				item.Next = null;
				item.Index = Tail.Index + 1;
				mList.Add(item);
				
				mCallback(item, item.Index);
			}
		}
	
		UpdatePosition();
		//Grid.GetComponent<UIGrid>().Reposition();

	}
	
	/// <summary>
	/// 아이템 모두 삭제한다.
	/// </summary>
	public void RemoveAll()
	{
		UIListItem item = null;
		for (int i = 0; i < mList.Count; i++)
		{
			item = mList[i];
			GameObject.DestroyImmediate(item.Target);
		}
		
		mList.Clear();
	}
	
	/// <summary>
	/// 해당 인덱스에 아이템을 추가한다.
	/// </summary>
	/// <param name="index"></param>
	public void AddItem(int index)
	{
		// 아직 필요없어서 추후 필요하면 구현 -_-)~....
	}
	
	/// <summary>
	/// scroll영역을 계산한다.
	/// </summary>
	/// <param name="root"></param>
	/// <param name="firstTemplate"></param>
	/// <param name="lastTemplate"></param>
	/// <returns></returns>
	private Bounds CalculateRelativeWidgetBounds2(Transform root, Transform firstTemplate, Transform lastTemplate)
	{
		if (firstTemplate == null || lastTemplate == null)
			return new Bounds(Vector3.zero, Vector3.zero);
		
		UIWidget[] widgets1 = firstTemplate.GetComponentsInChildren<UIWidget>(true) as UIWidget[];
		UIWidget[] widgets2 = lastTemplate.GetComponentsInChildren<UIWidget>(true) as UIWidget[];
		if (widgets1.Length == 0 || widgets2.Length == 0) return new Bounds(Vector3.zero, Vector3.zero);
		
		Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
	//	if(root == null) Utility.LogError("root");
		Matrix4x4 toLocal = root.worldToLocalMatrix;
		
		for (int i = 0, imax = widgets1.Length; i < imax; ++i)
		{
			UIWidget w = widgets1[i];
			Vector2 size = w.relativeSize;
			Vector2 offset = w.pivotOffset;
			Transform toWorld = w.cachedTransform;
			
			float x = (offset.x + 0.5f) * size.x;
			float y = (offset.y - 0.5f) * size.y;
			size *= 0.5f;
			
			// Start with the corner of the widget
			Vector3 v = new Vector3(x - size.x, y - size.y, 0f);
			
			// Transform the coordinate from relative-to-widget to world space
			v = toWorld.TransformPoint(v);
			
			// Now transform from world space to relative-to-parent space
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
			
			// Repeat for the other 3 corners
			v = new Vector3(x - size.x, y + size.y, 0f);
			v = toWorld.TransformPoint(v);
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
			
			v = new Vector3(x + size.x, y - size.y, 0f);
			v = toWorld.TransformPoint(v);
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
			
			v = new Vector3(x + size.x, y + size.y, 0f);
			v = toWorld.TransformPoint(v);
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
		}
		
		for (int i = 0, imax = widgets2.Length; i < imax; ++i)
		{
			UIWidget w = widgets2[i];
			Vector2 size = w.relativeSize;
			Vector2 offset = w.pivotOffset;
			Transform toWorld = w.cachedTransform;
			
			float x = (offset.x + 0.5f) * size.x;
			float y = (offset.y - 0.5f) * size.y;
			size *= 0.5f;
			
			// Start with the corner of the widget
			Vector3 v = new Vector3(x - size.x, y - size.y, 0f);
			
			// Transform the coordinate from relative-to-widget to world space
			v = toWorld.TransformPoint(v);
			
			// Now transform from world space to relative-to-parent space
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
			
			// Repeat for the other 3 corners
			v = new Vector3(x - size.x, y + size.y, 0f);
			v = toWorld.TransformPoint(v);
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
			
			v = new Vector3(x + size.x, y - size.y, 0f);
			v = toWorld.TransformPoint(v);
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
			
			v = new Vector3(x + size.x, y + size.y, 0f);
			v = toWorld.TransformPoint(v);
			v = toLocal.MultiplyPoint3x4(v);
			
			vMax = Vector3.Max(v, vMax);
			vMin = Vector3.Min(v, vMin);
		}
		
		Bounds b = new Bounds(vMin, Vector3.zero);
		b.Encapsulate(vMax);
		return b;
	}
}