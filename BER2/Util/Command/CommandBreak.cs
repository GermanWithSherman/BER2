using System.Collections;
using System.Collections.Generic;

public class CommandBreak : Command
{
    

    public override void execute(Data data)
    {
        Command.breakActive = true;
    }

    public override IModable copyDeep()
    {
        return new CommandBreak();
    }

    public override void mod(IModable modable){}
}
