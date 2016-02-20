namespace Orzoo.AspNet.Mvc
{
    public static class ControllerExtensions
    {
        //private static Feedback ExecuteSaveCore(Controller controller, Func<Feedback> func)
        //{
        //    var result = Feedback.Success();

        //    if (controller.ModelState.IsValid)
        //    {
        //        result = func();
        //    }

        //    // 将结果返回前端
        //    if (!controller.ModelState.IsValid)
        //    {
        //        result = Feedback.Fail(data: controller.ModelState.AllErrors());
        //    }
        //    return result;
        //}

        //private async static Task<Feedback> ExecuteSaveCore(Controller controller, Func<Task<Feedback>> func)
        //{
        //    var result = Feedback.Success();

        //    if (controller.ModelState.IsValid)
        //    {
        //        try
        //        {
        //        result = await func();
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }

        //    if (!controller.ModelState.IsValid)
        //    {
        //        result = Feedback.Fail(data: controller.ModelState.AllErrors());
        //    }
        //    return result;
        //}

        //public static Feedback ExecuteSave(this Controller controller, Func<Feedback> func)
        //{
        //    return ExecuteSaveCore(controller, func);
        //}

        //public async static Task<Feedback> ExecuteSaveAsync(this Controller controller, Func<Task<Feedback>> func)
        //{
        //    return await ExecuteSaveCore(controller, func);
        //}


        //public async static Task<Feedback> ExecuteSaveAsync(this Controller controller, Func<Task> func)
        //{
        //    return await ExecuteSaveCore(controller, async () =>
        //    {
        //        await func();
        //        return Feedback.Success();
        //    });
        //}


        //public static Feedback ExecuteSave(this Controller controller, Func<int> func)
        //{
        //    return
        //        ExecuteSaveCore(controller, () => func() > 0
        //        ? Feedback.Success() : Feedback.Fail("数据库保存失败"));

        //}

        //public static Feedback ExecuteSave(this Controller controller, Func<bool> func)
        //{
        //    return
        //        ExecuteSaveCore(controller, () => func()
        //        ? Feedback.Success() : Feedback.Fail("数据库保存失败"));

        //}

        //public async static Task<Feedback> ExecuteSaveAsync(this Controller controller, Func<Task<int>> func)
        //{
        //    return await ExecuteSaveCore(controller, async () => await func() > 0 ? Feedback.Success() : Feedback.Fail("数据库保存失败"));
        //}

        //public async static Task<Feedback> ExecuteSaveAsync(this Controller controller, Func<Task<Boolean>> func)
        //{
        //    return await ExecuteSaveCore(controller, async () => await func() ? Feedback.Success() : Feedback.Fail("数据库保存失败"));
        //}

        //public async static Task<Feedback> ExecuteSaveAsync(this Controller controller, Func<Task<DbResult>> func)
        //{
        //    return await ExecuteSaveCore(controller, async () =>
        //    {
        //        DbResult r = await func();
        //        controller.AddDbResultToModelState(r);
        //        return r.Succeeded ? Feedback.Success() : Feedback.Fail();
        //    });
        //}

        //public static void AddDbResultToModelState(this Controller controller, DbResult result)
        //{
        //    if (!result.Succeeded)
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            controller.ModelState.AddModelError(error.Type, error.Code + error.Description);
        //        }
        //    }
        //}
    }
}