using System;


namespace View.Utils.Dialog.Abstract;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }
}

