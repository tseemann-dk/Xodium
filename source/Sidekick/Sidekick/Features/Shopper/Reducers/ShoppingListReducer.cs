using System;
using System.Collections.Generic;
using Sidekick.Features.Shopper.Actions.ShoppingList;
using Sidekick.Features.Shopper.Models;
using Xodium.Flow;

namespace Sidekick.Features.Shopper.Reducers
{
    public static class ShoppingListReducer
    {
        private static readonly Dictionary<Type, Reducer<ShoppingList>> handlers = new Dictionary<Type, Reducer<ShoppingList>>
        {
            [typeof(AddComponentAction)] = (s, a) => AddComponent(s, (AddComponentAction)a),
            [typeof(AddGroupAction)] = (s, a) => AddGroup(s, (AddGroupAction)a),
            [typeof(AddItemAction)] = (s, a) => AddItem(s, (AddItemAction)a),
            [typeof(ChangeGroupTitleAction)] = (s, a) => ChangeGroupTitle(s, (ChangeGroupTitleAction)a),
            [typeof(DeleteNodeAction)] = (s, a) => DeleteNode(s, (DeleteNodeAction)a),
            [typeof(MoveNodeDownAction)] = (s, a) => MoveNodeDown(s, (MoveNodeDownAction)a),
            [typeof(MoveNodeUpAction)] = (s, a) => MoveNodeUp(s, (MoveNodeUpAction)a),
        };

        public static ShoppingList Execute(ShoppingList state, object action) =>
            handlers.TryGetValue(action.GetType(), out var handler) ? handler(state, action) : state;

        private static ShoppingList AddComponent(ShoppingList state, AddComponentAction action) =>
            state.AddComponent(action.Payload.Component);

        private static ShoppingList AddGroup(ShoppingList state, AddGroupAction action) =>
            state.AddNode(state.FindGroup(action.Payload.ParentGroupId), action.Payload.Group, action.Payload.InsertAfterNodeId);

        private static ShoppingList AddItem(ShoppingList state, AddItemAction action) =>
            state.AddNode(state.FindGroup(action.Payload.ParentGroupId), action.Payload.Item, action.Payload.InsertAfterNodeId);

        private static ShoppingList ChangeGroupTitle(ShoppingList state, ChangeGroupTitleAction action) => 
            state.ChangeGroupTitle(state.FindGroup(action.Payload.GroupId), action.Payload.NewTitle);

        private static ShoppingList DeleteNode(ShoppingList state, DeleteNodeAction action) =>
            state.DeleteNode(state.FindGroup(action.Payload.ParentGroupId), action.Payload.NodeId);

        private static ShoppingList MoveNodeDown(ShoppingList state, MoveNodeDownAction action) =>
            state.MoveNodeDown(state.FindGroup(action.Payload.ParentGroupId), action.Payload.NodeId);

        private static ShoppingList MoveNodeUp(ShoppingList state, MoveNodeUpAction action) =>
            state.MoveNodeUp(state.FindGroup(action.Payload.ParentGroupId), action.Payload.NodeId);
    }
}
