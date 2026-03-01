using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using YRFramework.Runtime;

namespace YRFramework.Editor
{
	[CanEditMultipleObjects] //没有该属性的编辑器在选中多个物体时会提示“Multi-object editing not supported”
    [CustomEditor(typeof(ReferenceCollector))] //自定义ReferenceCollector类在界面中的显示与功能
    public class ReferenceCollectorEditor : UnityEditor.Editor
    {
        private readonly StringBuilder stringBuilder = new();
        private ReferenceCollector referenceCollector;
        private Object heroPrefab;
        private string searchKey = "";

        /// <summary>
		/// 输入在textfield中的字符串
		/// </summary>
        private string SearchKey
		{
			get { return searchKey; }
			set
			{
				if (searchKey != value)
				{
					searchKey = value;
					heroPrefab = referenceCollector.Get<Object>(SearchKey);
				}
			}
		}

		private void OnEnable()
		{
			//将被选中的gameobject所挂载的ReferenceCollector赋值给编辑器类中的ReferenceCollector，方便操作
			referenceCollector = (ReferenceCollector)target;
		}

		public override void OnInspectorGUI()
		{
			//使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
			Undo.RecordObject(referenceCollector, "Changed Settings");
			SerializedProperty dataProperty = serializedObject.FindProperty("data");

			//开始水平布局，如果是比较新版本学习U3D的，可能不知道这东西，这个是老GUI系统的知识，除了用在编辑器里，还可以用在生成的游戏中
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("全部删除"))
				referenceCollector.Clear();

			if (GUILayout.Button("删除空引用"))
				DelNullReference(dataProperty);

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			//可以在编辑器中对searchKey进行赋值，只要输入对应的Key值，就可以点后面的删除按钮删除相对应的元素
			SearchKey = EditorGUILayout.TextField(SearchKey);
			//添加的可以用于选中Object的框，这里的object也是(UnityEngine.Object
			//第三个参数为是否只能引用scene中的Object
			EditorGUILayout.ObjectField(heroPrefab, typeof(Object), false);
			if (GUILayout.Button("删除"))
			{
				referenceCollector.Remove(SearchKey);
				heroPrefab = null;
			}
			GUILayout.EndHorizontal();

			EditorGUILayout.Space();

			List<int> delList = new();
			SerializedProperty property;
			//遍历ReferenceCollector中data list的所有元素，显示在编辑器中
			for (int i = referenceCollector.data.Count - 1; i >= 0; --i)
			{
				GUILayout.BeginHorizontal();

				//这里的知识点在ReferenceCollector中有说
				property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Key");
				EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
				property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("GameObject");
				property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Object), true);
				if (GUILayout.Button("X"))
					delList.Add(i);

				GUILayout.EndHorizontal();
			}

			EventType eventType = Event.current.type;
			//在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
			if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
			{
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

				if (eventType == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag();
					foreach (var o in DragAndDrop.objectReferences)
					{
						AddReference(dataProperty, o.name, o);
					}
				}

				Event.current.Use();
			}

			//遍历删除list，将其删除掉
			foreach (var i in delList)
			{
				dataProperty.DeleteArrayElementAtIndex(i);
			}
			serializedObject.ApplyModifiedProperties();
			serializedObject.UpdateIfRequiredOrScript();

			if (stringBuilder.Length > 0)
				EditorGUILayout.TextArea(stringBuilder.ToString());
		}

        private void DelNullReference(SerializedProperty dataProperty)
        {
            for (int i = dataProperty.arraySize - 1; i >= 0; --i)
            {
                SerializedProperty gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");
                if (gameObjectProperty.objectReferenceValue == null)
                {
                    dataProperty.DeleteArrayElementAtIndex(i);
                    EditorUtility.SetDirty(referenceCollector);
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.UpdateIfRequiredOrScript();
                }
            }
        }

        /// <summary>
		/// 添加元素
		/// </summary>
		/// <param name="dataProperty"></param>
		/// <param name="key"></param>
		/// <param name="obj"></param>
        private void AddReference(SerializedProperty dataProperty, string key, Object obj)
		{
			int index = dataProperty.arraySize;
			dataProperty.InsertArrayElementAtIndex(index);
			var element = dataProperty.GetArrayElementAtIndex(index);
			element.FindPropertyRelative("Key").stringValue = key;
			element.FindPropertyRelative("GameObject").objectReferenceValue = obj;
		}
	}
}