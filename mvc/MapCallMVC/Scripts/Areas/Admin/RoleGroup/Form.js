// This is the one example of how to import a module that we created via script bundling.
const { LitElement, html, when, ObservableArray } = await importMCLitComponentsModule();

class MCRoleGroupTable extends LitElement {

    // properties declared in here are automatically made reactive
    // by LitElement. Neat! Also note that you aren't setting default
    // values here. You're configuring how the properties work, instead.
    static properties = {
        // The hydrated data 
        applications: {},
        readonly: {
            type: Boolean,
            attribute: 'readonly'
        }
    };

    _applicationsByAppId = {};
    _applicationsByModuleId = {};
    _actionsById = {};
    _operatingCentersById = {};
    _statesById = {};
    _viewData = null;

    set viewData(viewData) {
        this._viewData = viewData;
        // Cache all this so we're not doing lookups constantly
        this._viewData.actions.forEach(x => this._actionsById[x.id] = x);
        this._viewData.operatingCenters.forEach(x => this._operatingCentersById[x.id] = x);
        this._viewData.states.forEach(x => this._statesById[x.id] = x);

        const parsedApps = [];
        viewData.applications.forEach((app) => {
            app.modules = {};
            app.roles = new ObservableArray();
            viewData.modules.forEach((module) => {
                if (module.applicationId == app.id) {
                    app.modules[module.id] = module;
                    this._applicationsByModuleId[module.id] = app;
                }
            });

            parsedApps.push(app);
            this._applicationsByAppId[app.id] = app;
        });

        // TODO: This needs to be sorted in alphabetical order.
        this.applications = parsedApps.sort((a, b) => {
            if (a.name > b.name) { return 1; }
            if (a.name < b.name) { return -1; }
            return 0;
        });
        this.tryAddManyRoleGroupRoles(viewData.existingRoles);

        this.requestUpdate();
    }

    get viewData() {
        return this._viewData;
    }

    get allRoleGroupRoles() {
        return this.applications.map(x => x.roles).flat();
    }

    constructor() {
        super();
    }

    // Lit, by default, renders everything to ShadowDOM. We don't
    // want that as none of our form elements and other things are
    // web components so we lose all of the styling and any js we
    // have for those won't get wired up properly.
    createRenderRoot() {
        return this;
    }

    /**
     * @description 
     * Adds multiple role group items at once. Rendering is only updated
     * once the roles are added. You should always bulk-add roles.
     * 
     * @param roleGroupRoles - array of role group items to be added. 
     * */
    tryAddManyRoleGroupRoles(roleGroupRoles) {
        if (!roleGroupRoles) {
            return;
        }

        const newRolesByApp = new Map();

        roleGroupRoles.forEach((roleGroupRole) => {
            const app = this._applicationsByModuleId[roleGroupRole.moduleId];
            const hasDuplicate = app.roles.find(x => x.moduleId === roleGroupRole.moduleId &&
                x.actionId === roleGroupRole.actionId &&
                x.operatingCenterId === roleGroupRole.operatingCenterId);

            if (hasDuplicate) {
                return;
            }

            if (!newRolesByApp.has(app)) {
                newRolesByApp.set(app, []);
            }

            // hydrate the role with some references for display purposes
            roleGroupRole.module = app.modules[roleGroupRole.moduleId];
            roleGroupRole.action = this._actionsById[roleGroupRole.actionId];
            if (roleGroupRole.operatingCenterId != null) {
                roleGroupRole.operatingCenter = this._operatingCentersById[roleGroupRole.operatingCenterId];
                roleGroupRole.state = this._statesById[roleGroupRole.operatingCenter.stateId];
            }

            newRolesByApp.get(app).push(roleGroupRole);
        });

        newRolesByApp.forEach((newRoles, app) => {
            app.roles.pushMany(newRoles);
        });
    }

    render() {
        return this.applications?.map((app) =>
            html`<mc-role-group-application-block 
                    class="container"
                    style="display:block;"
                    .application=${app}
                    .readonly=${this.readonly}>
                 </mc-role-group-application-block>`);
    }
}
customElements.define('mc-role-group-table', MCRoleGroupTable);

class MCRoleGroupApplicationBlock extends LitElement {

    static properties = {
        application: {},

        // This will need to hold all of the existing and new roles
        displayRoles: {},
        readonly: {
            type: Boolean
        }
    };

    _application = null;

    // In order to allow the top level component to update the ApplicationBlock
    // items from a single spot, we have to make our own property implementation
    // to subscribe to the ObservableArray. It's the only way to get binding
    // to work in this way.
    set application(val) {
        const oldVal = this._application
        if (oldVal && oldVal.roles) {
            oldVal.roles.unsubscribe(this.onRolesUpdated);
        }
        if (val) {
            val.roles.subscribe(this.onRolesUpdated);
        }
        this._application = val;
        this.requestUpdate('application', oldVal);
        // Call once so the roles get sorted.
        this.onRolesUpdated();
    }

    get application() {
        return this._application;
    }

    constructor() {
        super();
    }

    // Lit, by default, renders everything to ShadowDOM. We don't
    // want that as none of our form elements and other things are
    // web components so we lose all of the styling and any js we
    // have for those won't get wired up properly.
    createRenderRoot() {
        return this;
    }

    onRolesUpdated = () => {
        const sortBy = (a, b, valueAccessor) => {
            const aVal = valueAccessor(a);
            const bVal = valueAccessor(b);
            if (aVal > bVal) { return 1; }
            if (aVal < bVal) { return -1; }
            return 0;
        };

        // Need to copy the roles to a new array to force the displayRoles
        // binding to update.
        const roles = [...this.application.roles].sort((a, b) => {
            return sortBy(a, b, x => x.module.name) ||
                sortBy(a, b, x => x.operatingCenter?.name) ||
                sortBy(a, b, x => x.action.name);
        });
        this.displayRoles = roles;
    }

    onSelectAllRolesChanged = (e) => {
        const isChecked = e.target.checked;
        this.application.roles.forEach(x => {
            x.mustBeRemoved = isChecked;
        });
        // need to force update rendering for this since the property
        // being changed isn't a direct property of this component.
        this.requestUpdate();
    }

    // NOTE: This compponent has to render the table rows. We can't use another component for
    // that as it wouldn't generate a valid html table(the rows would be wrapped with the component).
    render() {
        if (this.displayRoles.length === 0) {
            return;
        }

        // NOTE: the details tag is being closed by default. Otherwise, performance gets very sluggish
        // when dealing with thousands of tables rows.
        const result = html`<details id=${"details-" + this.application.name}>
                        <summary>${this.application.name} - ${this.displayRoles.length} roles</summary>
                        <div class="container">
                            <table id=${"application-" + this.application.name} style="width:100%; table-layout:fixed;">
                                <thead>
                                    <tr>
                                        <th>Module</th>
                                        <th>State</th>
                                        <th>Operating Center</th>
                                        <th>Action</th>
                                       ${when(!this.readonly,
                                           () => html`<th><label>Select All <input type="checkbox" 
                                                                                   id=${"select-all-" + this.application.name}
                                                                                   @input=${this.onSelectAllRolesChanged} /></label></th>`,
            () => html``)}
                                    </tr>
                                </thead>
                                <tbody>
                                ${this.displayRoles.map((role) => this.renderRoleGroupRow(role))}
                                </tbody>
                            </table>
                        </div>
                     </details>`;
        return result;
    }

    renderRoleGroupRow(roleGroupRole) {
        // We can't use the regular lit.js style definitions for this because we're
        // not using ShadowDOM. So we need to shove the repeating style into the rendered
        // output instead. This isn't recommended, but I'm trying to keep this all in one
        // spot rather than scattered in different files.
        //
        // NOTE: The name and value attributes on the remove checkbox are only here for
        // functional tests. They have no practical use.
        const cssClass = (roleGroupRole.id === undefined) ? 'unsaved-role' : null;
        const result = html`
                <style>
                    tr.unsaved-role td {
                        background-color: #b3d86a !important;
                    }
                </style>
                <tr class=${cssClass}>
                    <td>${roleGroupRole.module.name}</td>
                    <td>${roleGroupRole.state?.name || 'ALL'}</td>
                    <td>${roleGroupRole.operatingCenter?.name || 'ALL'}</td>
                    <td>${roleGroupRole.action.name}</td>
                    ${when(!this.readonly,
                         () => html`<td>
                                        <label>
                                            Remove
                                            <input type="checkbox"
                                                   name="RolesToRemove"
                                                   value=${roleGroupRole.id}
                                                   .checked=${roleGroupRole.mustBeRemoved} 
                                                   @input=${e => roleGroupRole.mustBeRemoved = !roleGroupRole.mustBeRemoved}></input>
                                        </label>
                                    </td>`,
                        () => html``)
                    }
                </tr>`;
        return result;
    }
}
customElements.define('mc-role-group-application-block', MCRoleGroupApplicationBlock);

/* @description
 * This is a hack around not being able to easily do a json save request due
 * to SecureForms not having a way of doing json stuff yet.
 */
class MCRoleGroupFormFields extends LitElement {

    static properties = {
        allRoles: {}
    };

    constructor() {
        super();
    }

    // Lit, by default, renders everything to ShadowDOM. We don't
    // want that as none of our form elements and other things are
    // web components so we lose all of the styling and any js we
    // have for those won't get wired up properly.
    createRenderRoot() {
        return this;
    }

    render() {
        if (this.allRoles?.length === undefined || this.allRoles?.length === 0) {
            return;
        }
        const data = {
            newRoles: this.allRoles.filter(x => !x.id && !x.mustBeRemoved).map(x => {
                return {
                    module: x.module.id,
                    action: x.action.id,
                    operatingCenter: x.operatingCenter?.id
                };
            }),
            rolesToRemove: this.allRoles.filter(x => x.mustBeRemoved && x.id).map(x => x.id)
        };

        const newRoles = html`${data.newRoles.map((roleGroupRole, index) => {
            return html`<input type="hidden" name="NewRoles[${index}].Action" value=${roleGroupRole.action} />
                        <input type="hidden" name="NewRoles[${index}].Module" value=${roleGroupRole.module} />
                        <input type="hidden" name="NewRoles[${index}].OperatingCenter" value=${roleGroupRole.operatingCenter} />`;
        })}`;

        // NOTE: Indexer not needed for removal.
        const rolesToRemove = html`${data.rolesToRemove.map((roleGroupRoleId) => {
            return html`<input type="hidden" name="RolesToRemove" value=${roleGroupRoleId} />`;
        })}`;

        return html`${newRoles} ${rolesToRemove}`;
    }
}
customElements.define('mc-role-group-form-fields-block', MCRoleGroupFormFields);

(($) => {
    const ELEMENTS = {
        addRolesDialog: $('#add-roles-dialog'),
        addRolesForm: $('#add-roles-form'),
        rolesPanel: $('#roles-panel'),
        roleGroupForm: $('#role-group-form')
    };

    const initSelectAllRolesPerAppCheckboxes = () => {
        $('.select-all-roles-per-app-checkbox').on('input', function () {
            const removePerAppCheckbox = $(this);
            const removableCheckboxes = removePerAppCheckbox.closest('table').find('input[name=RolesToRemove]');
            removableCheckboxes.prop('checked', removePerAppCheckbox.prop('checked'));
        });
    };

    const onSubmitRoleGroupForm = (e) => {
        // This is a hack to generate a bunch of hidden fields for the form to use
        // so we can continue using SecureForms and the typical ActionHelper pattern.
        const allRoles = ELEMENTS.rolesPanel[0].allRoleGroupRoles;

        const newRolesCount = allRoles.filter(x => !x.id && !x.mustBeRemoved).length;
        const rolesToRemoveCount = allRoles.filter(x => x.mustBeRemoved && x.id).length;

        // We only care about the confirmation message if there's changes to roles. 
        // If they only change the role group name, it doesn't matter.
        if ((newRolesCount || rolesToRemoveCount) &&
            !confirm(`You're adding ${newRolesCount} and removing ${rolesToRemoveCount} roles. 
Are you sure you want to save these changes? These changes will immediately apply to all users with this role group.`)) {
            return false;
        }

        const dynamicFormFieldsEl = document.getElementById('role-group-dynamic-form-fields');
        dynamicFormFieldsEl.allRoles = allRoles;
    }

    const onSaveRolesButtonClicked = (e) => {
        // We don't want the form to actually submit because it's
        // hacked together using a view with a form for validation
        // purposes. But we're not posting back the form here, just
        // using the values.
        e.preventDefault();

        if (!ELEMENTS.addRolesForm.valid()) {
            return;
        }

        const getCheckedValues = (elementId) => {
            const arr = [];
            $(`${elementId} mc-checkboxlistitem input:checked`).each((i, item) => arr.push(parseInt(item.value)));
            return arr;
        }
        const modules = getCheckedValues('#Modules');
        const isForAllOperatingCenters = $('#IsForAllOperatingCenters').is(':checked');
        const operatingCenters = getCheckedValues('#OperatingCenters');
        const actions = getCheckedValues('#Actions');
        const rolesPanel = ELEMENTS.rolesPanel[0];
        const modulesToAdd = [];

        modules.forEach(moduleId => {
            actions.forEach(actionId => {
                if (isForAllOperatingCenters) {
                    modulesToAdd.push({
                        moduleId: moduleId,
                        actionId: actionId,
                        operatingCenterId: null
                    });
                }
                else {
                    operatingCenters.forEach(operatingCenterId => {
                        modulesToAdd.push({
                            moduleId: moduleId,
                            actionId: actionId,
                            operatingCenterId: operatingCenterId
                        });
                    })
                }
            });
        })

        // Adding these at once reduces the amount of redraws being requested
        // down to 1 instead of 1 for each item.
        rolesPanel.tryAddManyRoleGroupRoles(modulesToAdd);

        ELEMENTS.addRolesDialog[0].close();
    }

    const initRolesPanel = () => {
        ELEMENTS.rolesPanel[0].viewData = window.ROLE_GROUP_VIEW_DATA;
    }

    const init = () => {
        initRolesPanel();
        initSelectAllRolesPerAppCheckboxes();
        ELEMENTS.addRolesForm.on('submit', onSaveRolesButtonClicked);
        ELEMENTS.roleGroupForm.on('submit', onSubmitRoleGroupForm);
    };

    $(document).ready(init);
})(jQuery, window.operatingCentersByState);