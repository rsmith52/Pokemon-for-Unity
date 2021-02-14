using UnityEditor;
using UnityEngine;
using Eventing;

[CustomPropertyDrawer(typeof(EventEntry))]
public class EventEntryEditor : PropertyDrawer
{
    //I use PropertyDrawer because Phase does not derived from MonoBehavior
    //I don'tuse monobehavior because I want Phase to be serializable


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //set height
        position.height = 20; // If you want line to be closer to each other in y direction, change here.

        //-----Effect Type-----
        //get effect property
        var effect_type_property = property.FindPropertyRelative("effect");
        //make a new movementType variable as a Phase.PhaseMovementType and set it equal to the property after convert
        Effects effect_type = (Effects)effect_type_property.enumValueIndex; //get Index and type cast it back to enum
        //create popup to get the new value
        effect_type = (Effects)EditorGUI.EnumPopup(position, "", effect_type);//""(label) is important. Else the title of the popup will appear
        //send back the new value
        effect_type_property.enumValueIndex = (int)effect_type;
        //position.y += position.height;//add height after movementType field (not needed if the label is "")

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        //using property.isExpanded is very convenient, but there's a catch. I can not have multiple foldout, because there's only 1 variable.
        //If I want multiple foldout, I may need a dictionary or something to store it instead.
        //However, since I only want 1 foldout (setting) that differ through each enum, it will works fine for now.

        //the foldout will be different for every type of movementType
        if (property.isExpanded)
        {
            position.y += position.height;
            if (effect_type == Effects.ShowMessage)
            {
                // properties special for ShowMessage
                var message_property = property.FindPropertyRelative("message");
                message_property.stringValue = EditorGUI.TextField(position, "message", message_property.stringValue);
                position.y += position.height;
            }
            else if (effect_type == Effects.ShowChoices)
            {
                // properties special for ShowChoices
                var message_property = property.FindPropertyRelative("message");
                message_property.stringValue = EditorGUI.TextField(position, "message", message_property.stringValue);
                position.y += position.height;

                var choice_package_property = property.FindPropertyRelative("choice_package");
                choice_package_property.objectReferenceValue = EditorGUI.ObjectField(position, choice_package_property.objectReferenceValue);
                position.y += position.height;
            }
        }
    }

    //GetPropertyHeight is like getting the height of the whole box of script in the inspector before filling in
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? 20 * 3 : 20 * 1;
        //kind of inconvenient but works for now. If I have more variable I have to adjust these number myself.
    }
}