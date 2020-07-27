﻿using System.Collections;
using System.Collections.Generic;

public class CommandNone : Command
{
    public override void execute(Data data){}

    public override IModable copyDeep()
    {
        return new CommandNone();
    }

    public override void mod(IModable modable) { }
}
